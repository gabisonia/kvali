namespace Kvali.AuditTrail;

/// <summary>
/// Attribute to mark an entity as auditable. Entities with this attribute will have changes logged in the audit trail.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AuditableAttribute : Attribute
{
}