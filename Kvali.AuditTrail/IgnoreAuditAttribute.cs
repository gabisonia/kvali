namespace Kvali.AuditTrail;

/// <summary>
/// Attribute to mark a property as ignored for auditing purposes. Changes to this property will not be logged.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class IgnoreAuditAttribute : Attribute
{
}