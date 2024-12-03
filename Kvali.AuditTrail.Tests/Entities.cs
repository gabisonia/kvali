namespace Kvali.AuditTrail.Tests;

// [Auditable]
public class Entities : IAuditable
{
    public int Id { get; set; }
    public string Name { get; set; }
    [IgnoreAudit] public decimal Price { get; set; }
}

// Only audits on create.
[AuditAction(AuditActionType.Create)]
public class User : IAuditable
{
    public int Id { get; set; }
    public string Name { get; set; }
}