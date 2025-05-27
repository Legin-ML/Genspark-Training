using System.ComponentModel.DataAnnotations;

namespace NotTwitter.Models;

public class Hashtag
{
    [Key]
    public int HashtagId { get; set; }
    
    [Required]
    public string HashtagContent { get; set; }
    
    public ICollection<MapHashtags> Hashtags { get; set; } = new List<MapHashtags>();
}