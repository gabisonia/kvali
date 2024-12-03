namespace Kvali.AuditTrail;

/// <summary>
/// Represents an entry in the audit trail, capturing details of changes made to entities.
/// </summary>
public class AuditEntry
{
    /// <summary>
    /// Gets or sets the unique identifier for the audit entry.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the entity that was changed.
    /// </summary>
    public string EntityName { get; set; }

    /// <summary>
    /// Gets or sets the original value of the property before the change.
    /// </summary>
    public string? OldValue { get; set; }

    /// <summary>
    /// Gets or sets the new value of the property after the change.
    /// </summary>
    public string? NewValue { get; set; }

    /// <summary>
    /// Gets or sets the datetime when the change was made.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the action performed on the entity, such as Create, Update, or Delete.
    /// </summary>
    public string Action { get; set; }

    /// <summary>
    /// Gets or sets the entity id.
    /// </summary>
    public string? EntityId { get; set; }
}