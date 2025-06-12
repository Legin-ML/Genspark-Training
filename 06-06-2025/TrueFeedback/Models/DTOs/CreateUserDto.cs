using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    public string UserName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string Password { get; set; }
    public string RoleName { get; set; }
}