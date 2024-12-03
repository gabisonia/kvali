namespace Kvali.AuditTrail;

/// <summary>
/// Attribute used to specify which type of actions should trigger auditing for a class.
/// </summary>
/// <remarks>
/// Use this attribute to restrict auditing to specific actions (e.g., Create, Update, Delete)
/// for a given property in an auditable entity.
/// </remarks>
[AttributeUsage(AttributeTargets.Class)]
public class AuditActionAttribute(AuditActionType actionType) : Attribute
{
    public AuditActionType ActionType { get; } = actionType;
}