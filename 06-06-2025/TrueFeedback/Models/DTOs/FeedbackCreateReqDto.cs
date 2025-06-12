public class FeedbackCreateReqDto
{
    public Guid UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public int Rating { get; set; } 
}