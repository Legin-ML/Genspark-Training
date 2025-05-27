using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotTwitter.Models;

public class MapHashtags
{
    [Key]
    public int Id { get; set; }

    [Required] 
    public int TweetId { get; set; }
    [Required]
    public int HashtagId { get; set; }
    
    [ForeignKey(nameof(TweetId))]
    public Tweet Tweet {get; set;}
    
    [ForeignKey(nameof(HashtagId))]
    public Hashtag Hashtag { get; set; }
}