# Kvali

Kvali (Georgian: **კვალი**) is a lightweight and flexible .NET Core library for implementing audit trails in your applications. It helps you track changes, log actions, and maintain a detailed history of operations for auditing and debugging purposes.

## Features

- Seamless integration with .NET Core applications.
- Tracks and logs create, update, and delete operations.
- Stores comprehensive audit data, including datetime, user actions, and object changes.
- Configurable and extensible for custom audit requirements.
- Lightweight and performance-oriented.


## Usage

### 1. Inherit `AuditableDbContext`

To enable auditing in your DbContext, inherit from `AuditableDbContext` instead of `DbContext`. Here's an example:

```csharp
public class TestDbContext : AuditableDbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) 
        : base(options) { }

    public DbSet<Entities> Products { get; set; }
    public DbSet<User> Users { get; set; }
}
```

### 2. Annotate Your Models for Auditing

```csharp
[Auditable]
public class Entities
{
    public int Id { get; set; }
    public string Name { get; set; }

    // This property will be ignored during auditing.
    [IgnoreAudit]
    public decimal Price { get; set; }
}
```
Or

```csharp
public class Entities : IAuditable
{
    public int Id { get; set; }
    public string Name { get; set; }

    // This property will be ignored during auditing.
    [IgnoreAudit]
    public decimal Price { get; set; }
}
```

### 3. Action-Specific Auditing
```csharp
// Only audits on create actions.
[AuditAction(AuditActionType.Create)]
public class User : IAuditable
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

## License

Kvali is fully free to use, modify, and distribute for any purpose, personal or commercial. 

No attribution is required, but contributions and feedback are always welcome!
