using Microsoft.EntityFrameworkCore;

namespace Kvali.AuditTrail.Tests;

public class AuditableDbContextTests
{
    private DbContextOptions<AuditableDbContext> CreateInMemoryOptions()
    {
        return new DbContextOptionsBuilder<AuditableDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void SaveChanges_ShouldLogCreateAction()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new TestDbContext(options);

        var product = new Entities { Name = "Test Product", Price = 10.99M };

        // Act
        context.Add(product);
        context.SaveChanges();

        // Assert
        var auditLogs = context.AuditLogs.ToList();
        Assert.Single(auditLogs);
        var log = auditLogs.First();
        Assert.Equal("Create", log.Action);
        Assert.Contains("Test Product", log.NewValue);
        Assert.Null(log.OldValue);
    }

    [Fact]
    public void SaveChanges_ShouldLogUpdateAction()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new TestDbContext(options);

        var product = new Entities { Name = "Test Product", Price = 10.99M };
        context.Add(product);
        context.SaveChanges();

        // Act
        product.Name = "Updated Product";
        context.Update(product);
        context.SaveChanges();

        // Assert
        var log = context.AuditLogs.First(x => x.Action == "Update");
        Assert.Equal("Update", log.Action);
        Assert.Contains("Updated Product", log.NewValue);
        Assert.Contains("Test Product", log.OldValue);
    }

    [Fact]
    public void SaveChanges_ShouldLogDeleteAction()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new TestDbContext(options);

        var product = new Entities { Name = "Test Product", Price = 10.99M };
        context.Add(product);
        context.SaveChanges();

        // Act
        context.Remove(product);
        context.SaveChanges();

        // Assert
        var log = context.AuditLogs.First(x => x.Action == "Delete");
        Assert.Equal("Delete", log.Action);
        Assert.Contains("Test Product", log.OldValue);
        Assert.Null(log.NewValue);
    }

    [Fact]
    public void SaveChanges_ShouldIgnorePropertiesWithIgnoreAuditAttribute()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new TestDbContext(options);

        var product = new Entities { Name = "Test Product", Price = 10.99M };
        context.Add(product);
        context.SaveChanges();

        // Act
        product.Price = 15.99M; // Price is marked with [IgnoreAudit]
        context.Update(product);
        context.SaveChanges();

        // Assert
        var auditLogs = context.AuditLogs.ToList();
        Assert.All(auditLogs, log => Assert.DoesNotContain("Price", log.NewValue));
    }

    [Fact]
    public void SaveChanges_ShouldAuditUserEntityOnlyOnCreate()
    {
        // Arrange
        var options = CreateInMemoryOptions();
        using var context = new TestDbContext(options);

        var user = new User { Id = 1, Name = "Test User" };

        // Act
        context.Add(user);
        context.SaveChanges();

        user.Name = "Updated User";
        context.Update(user);
        context.SaveChanges();

        // Assert
        var auditLogs = context.AuditLogs.ToList();

        Assert.Single(auditLogs);
        Assert.All(auditLogs, log => Assert.Contains("Create", log.Action));
    }
}