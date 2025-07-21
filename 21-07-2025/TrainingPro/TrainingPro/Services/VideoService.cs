using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;

namespace TrainingPro.Services;

public class VideoService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _config;
    private readonly string _containerName = "training-videos";

    public VideoService(BlobServiceClient blobServiceClient, IConfiguration config)
    {
        _blobServiceClient = blobServiceClient;
        _config = config;
    }

    public async Task UploadFileAsync(Stream fileStream, string fileName)
    {
        if (fileStream == null || string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Invalid input");

        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync();

        var blobClient = containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, overwrite: true);
    }

    public async Task<Stream> GetFileStreamAsync(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name is required");

        var blobClient = _blobServiceClient.GetBlobContainerClient(_containerName)
                                           .GetBlobClient(fileName);

        if (await blobClient.ExistsAsync())
        {
            var stream = await blobClient.OpenReadAsync(); 
            return stream;
        }

        throw new FileNotFoundException($"File '{fileName}' not found in blob storage.");
    }

    public async Task<bool> FileExistsAsync(string fileName)
    {
        var blobClient = _blobServiceClient.GetBlobContainerClient(_containerName)
                                           .GetBlobClient(fileName);
        return await blobClient.ExistsAsync();
    }

    public Uri GetFileUrl(string fileName)
    {
        var blobClient = _blobServiceClient.GetBlobContainerClient(_containerName)
                                           .GetBlobClient(fileName);
        var sasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(1));
        return sasUri;
    }
}
