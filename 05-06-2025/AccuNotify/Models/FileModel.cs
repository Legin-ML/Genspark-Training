using System.ComponentModel.DataAnnotations;

namespace AccuNotify.Models;

public class FileModel
{
    [Key]
    public int Id { get; set; }
    public string FileName { get; set; }
    public byte[] FileData { get; set; }
    public string FileType { get; set; }
}