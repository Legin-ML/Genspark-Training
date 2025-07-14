using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BlobStorage.Interfaces;

namespace BlobStorage.Services;

using Azure.Storage.Blobs;

public class FileService : IFileService
{
    private readonly BlobContainerClient _containerClient;

    public FileService(IConfiguration configuration)
    {
        var vault = configuration["AzureKeyVault:VaultUrl"];
        var keyName = configuration["AzureKeyVault:SecretName"];
        
        SecretClient secretClient = new SecretClient(new Uri(vault), new DefaultAzureCredential());
        
        KeyVaultSecret secret = secretClient.GetSecret(keyName);
        string sasUrl = secret.Value;
        _containerClient = new BlobContainerClient(new Uri(sasUrl));
    }

    public async Task UploadFile(Stream fileStream, string fileName)
    {
        if (fileStream == null || string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Invalid input");

        var blobClient = _containerClient.GetBlobClient(fileName);
        await blobClient.UploadAsync(fileStream, overwrite: true);
    }

    public async Task<Stream> GetFile(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("File name is required");

        var blobClient = _containerClient.GetBlobClient(fileName);

        if (await blobClient.ExistsAsync())
        {
            var downloadInfo = await blobClient.DownloadStreamingAsync();
            return downloadInfo.Value.Content;
        }

        throw new FileNotFoundException($"File '{fileName}' not found in blob storage.");
    }

    public async Task<bool> FileExists(string fileName)
    {
        var blobClient = _containerClient.GetBlobClient(fileName);
        return await blobClient.ExistsAsync();
    }
}
