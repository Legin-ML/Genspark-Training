using TrueFeedback.Enums;
using TrueFeedback.Interfaces;

namespace TrueFeedback.Models;

public class AuditLog : IEntity
{
    public Guid Id { get; set; }
    public OperationType  OperationType { get; set; }
    public DateTime OperationTime { get; set; }
    public Guid UserId { get; set; }
    public string Status { get; set; }
    public string Message { get; set; }
}