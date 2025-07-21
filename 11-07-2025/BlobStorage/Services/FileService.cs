using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BlobStorage.DTOs;
using BlobStorage.Interfaces;

namespace BlobStorage.Services;

using Azure.Storage.Blobs;

public class FileService : IFileService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public FileService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }
    
    private async Task<BlobClient> GetBlobClientWithSas(string fileName)
    {
        string functionUrl = $"https://legingensasfunc.azurewebsites.net/api/generate-sas/{fileName}";
        var client = _httpClientFactory.CreateClient();
        var sasResponse = await client.GetAsync(functionUrl);
        if (!sasResponse.IsSuccessStatusCode)
        {
            var error = await sasResponse.Content.ReadAsStringAsync();
            throw new InvalidOperationException("Could not obtain SAS URL.");
        }

        var sasData = await sasResponse.Content.ReadFromJsonAsync<SasResponse>();
        if (sasData == null || string.IsNullOrWhiteSpace(sasData.sasUrl))
        {
            throw new InvalidOperationException("SAS URL response invalid.");
        }

        // Create BlobClient directly using the SAS URL
        return new BlobClient(new Uri(sasData.sasUrl));
    }

    public async Task UploadFile(Stream fileStream, string fileName)
    {
        if (fileStream == null || string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Invalid input");

        var blobClient = await GetBlobClientWithSas(fileName);
        await blobClient.UploadAsync(fileStream, overwrite: true);
    }

    public async Task<Stream> GetFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name is required");

        var blobClient = await GetBlobClientWithSas(fileName);

        if (await blobClient.ExistsAsync())
        {
            var downloadInfo = await blobClient.DownloadStreamingAsync();
            return downloadInfo.Value.Content;
        }

        throw new FileNotFoundException($"File '{fileName}' not found in blob storage.");
    }

    public async Task<bool> FileExists(string fileName)
    {
        var blobClient = await GetBlobClientWithSas(fileName);
        return await blobClient.ExistsAsync();
    }
}
