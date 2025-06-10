namespace TrueFeedback.Models;

public class Feedback
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Message { get; set; }
    public float Rating { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public bool IsDeleted { get; set; }
}