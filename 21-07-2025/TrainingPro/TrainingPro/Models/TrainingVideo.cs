using System.ComponentModel.DataAnnotations;

namespace TrainingPro.Models;

public class TrainingVideo
{
    [Required]
    public int Id { get; set; }
    [Required]
    [StringLength(50)]
    public string Title { get; set; }
    [Required]
    [StringLength(200)]
    public string Description { get; set; }

    public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    public string BlobFileUri { get; set; }
    
}