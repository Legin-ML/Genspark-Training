using TrueFeedback.Interfaces;

namespace TrueFeedback.Models;

public class User : IEntity
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    public string? RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExpiryTime { get; set; }
    
    
    public Role Role { get; set; }
}