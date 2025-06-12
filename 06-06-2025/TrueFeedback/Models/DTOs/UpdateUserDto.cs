using System.ComponentModel.DataAnnotations;

public class UpdateUserDto
{
    [EmailAddress]
    public string Email { get; set; }
    public string UserName { get; set; }
    
    public string Password { get; set; }
    
    public string RoleName { get; set; }
}