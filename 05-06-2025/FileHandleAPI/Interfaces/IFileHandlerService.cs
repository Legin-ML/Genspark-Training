namespace FileHandleAPI.Interfaces;

public interface IFileHandlerService
{
    Task<byte[]> GetFile(string fileName);
    Task UploadFile(IFormFile file);
}