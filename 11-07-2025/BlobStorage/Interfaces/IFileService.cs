namespace BlobStorage.Interfaces;

public interface IFileService
{
    Task<Stream> GetFile(string fileName);
    Task UploadFile(Stream fileStream, string fileName);
}