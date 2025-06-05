using FileHandleAPI.Interfaces;

namespace FileHandleAPI.Services;

public class FileHandlerService : IFileHandlerService
{
    private readonly string _localDirectory;

    public FileHandlerService()
    {
        _localDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Files");
        if(!Directory.Exists(_localDirectory))
            Directory.CreateDirectory(_localDirectory);
    }

    public async Task<byte[]> GetFile(string fileName)
    {
        var filePath = Path.Combine(_localDirectory, fileName);

        if (!File.Exists(filePath))
            throw new FileNotFoundException();

        return await File.ReadAllBytesAsync(filePath);
    }
    
    public async Task UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentNullException("Invalid file");
        try
        {
            var newFile = Path.Combine(_localDirectory, file.FileName);

            using var readStream = new FileStream(newFile, FileMode.Create);
            await file.CopyToAsync(readStream);
        }
        catch (Exception ex)
        {
            throw new Exception("Error uploading file", ex);
        }
    }
}