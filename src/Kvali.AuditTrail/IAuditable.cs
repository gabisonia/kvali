namespace Kvali.AuditTrail;

/// <summary>
/// Marker interface for entities that should be included in the audit trail.
/// </summary>
/// <remarks>
/// Entities implementing this interface will be tracked automatically for changes
/// (e.g., Create, Update, Delete) and their modifications will be logged in the audit trail.
/// </remarks>
public interface IAuditable
{
}