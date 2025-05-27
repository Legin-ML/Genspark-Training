using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotTwitter.Models;

public class Follow
{
    [Key]
    public int FollowId { get; set; }
    
    [Required]
    public int FollowerId { get; set; }
    
    [Required]
    public int FolloweeId { get; set; }
    
    [ForeignKey(nameof(FollowerId))]
    [InverseProperty("Following")]
    public User Follower { get; set; }
    
    [ForeignKey(nameof(FolloweeId))]
    [InverseProperty("Followers")]
    public User Followee { get; set; }
            
}