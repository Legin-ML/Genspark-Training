using System.ComponentModel.DataAnnotations;

namespace TrainingPro.Models.DTOs;

public class VideoUpload
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Required]
    public IFormFile File { get; set; }
}

