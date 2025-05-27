using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotTwitter.Models;

public class Tweet
{
    [Key]
    public int TweetId { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    [Required]
    public DateTime CreatedTime {get; set;} = DateTime.UtcNow;
    
    public DateTime UpdatedTime {get; set;} = DateTime.UtcNow;
    
    [ForeignKey(nameof(UserId))]
    public User User {get; set;}
    
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public ICollection<MapHashtags>  hashtags { get; set; } = new List<MapHashtags>();
}