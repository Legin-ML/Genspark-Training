using TrueFeedback.Interfaces;

namespace TrueFeedback.Models;

public class Feedback : IEntity
{ 
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? ReplyToId { get; set; }
    public string Message { get; set; }
    public string? Reply { get; set; }
    public float Rating { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public bool IsDeleted { get; set; }
}