namespace Kvali.AuditTrail;

/// <summary>
/// Represents the type of action performed on an entity in the audit trail.
/// </summary>
public enum AuditActionType : byte
{
    Create = 1,
    Update = 2,
    Delete = 3
}