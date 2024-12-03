using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Kvali.AuditTrail;

/// <summary>
/// A custom DbContext that includes auditing capabilities for tracking changes to auditable entities.
/// </summary>
public class AuditableDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AuditEntry> AuditLogs { get; set; }

    public override int SaveChanges()
    {
        var auditEntries = GetAuditEntries(ChangeTracker);

        AuditLogs.AddRange(auditEntries);

        return base.SaveChanges();
    }

    private List<AuditEntry> GetAuditEntries(ChangeTracker changeTracker)
    {
        var auditEntries = new List<AuditEntry>();

        foreach (var entry in changeTracker.Entries())
        {
            var entryType = entry.Entity.GetType();

            var isAuditable = entryType.GetCustomAttributes(typeof(AuditableAttribute), true).Any() ||
                              entry.Entity is IAuditable;
            if (!isAuditable)
                continue;

            var entityName = entryType.Name;
            var actionType = entry.State switch
            {
                EntityState.Added => "Create",
                EntityState.Deleted => "Delete",
                EntityState.Modified => "Update",
                _ => null
            };
            // Skip entries that are not Added or Deleted
            if (actionType == default)
                continue;

            if (entry.Entity.GetType()
                    .GetCustomAttributes(typeof(AuditActionAttribute), true)
                    .FirstOrDefault() is AuditActionAttribute auditActionAttribute &&
                auditActionAttribute.ActionType.ToString() != actionType)
                continue;

            var entityType = entry.Context.Model.FindEntityType(entry.Entity.GetType());
            var primaryKey = entityType?.FindPrimaryKey();
            var keyValues = primaryKey?.Properties
                .Select(p => entry.Property(p.Name).CurrentValue ?? entry.Property(p.Name).OriginalValue)
                .ToArray();

            var auditEntry = new AuditEntry
            {
                EntityName = entityName,
                OldValue = entry.State is EntityState.Deleted or EntityState.Modified
                    ? Serialize(entry.OriginalValues)
                    : default,
                NewValue = entry.State is EntityState.Added or EntityState.Modified
                    ? Serialize(entry.CurrentValues)
                    : default,
                Action = actionType,
                CreatedAt = DateTimeOffset.UtcNow,
                EntityId = keyValues != default
                    ? keyValues is { Length: 1 }
                        ? keyValues[0]?.ToString()
                        : string.Join(",", keyValues)
                    : default
            };

            auditEntries.Add(auditEntry);
        }

        return auditEntries;
    }

    private string? Serialize(PropertyValues? values)
    {
        if (values == null)
            return null;

        var dictionary = values.Properties
            .Where(p => p.PropertyInfo?.GetCustomAttribute<IgnoreAuditAttribute>() ==
                        null)
            .ToDictionary(p => p.Name, p => values[p.Name]);

        return JsonSerializer.Serialize(dictionary);
    }
}