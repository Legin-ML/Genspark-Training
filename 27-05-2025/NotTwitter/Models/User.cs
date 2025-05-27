using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotTwitter.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    [Required]
    [MaxLength(50)]
    public string Username { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string PasswordHash { get; set; }

    [Required] 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public string Status {get; set;}

    public ICollection<Tweet> Tweets { get; set; } = new List<Tweet>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    
    [InverseProperty("Followee")]
    public ICollection<Follow> Followers { get; set; } = new List<Follow>();
    [InverseProperty("Follower")]
    public ICollection<Follow> Following { get; set; } = new List<Follow>();
    

}