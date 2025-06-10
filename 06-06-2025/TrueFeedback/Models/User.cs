namespace TrueFeedback.Models;

public class User
{
    public Guid Id { get; set; }
    public int RoleId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsDeleted { get; set; }
}