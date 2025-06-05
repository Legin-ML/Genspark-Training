using AccuNotify.Hubs;
using AccuNotify.Models;
using AccuNotify.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace AccuNotify.Services;

public class FileService
{
    private readonly FileModelRepository _fileRepository;
    private readonly IHubContext<NotifyHub> _hubContext;

    public FileService(FileModelRepository fileRepository, IHubContext<NotifyHub> hubContext)
    {
        _fileRepository = fileRepository;
        _hubContext = hubContext;
    }

    public async Task<FileModel> GetFileAsync(int id)
    {
        return await _fileRepository.GetAsync(id);
    }

    public async Task<FileModel> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentNullException(nameof(file), "Invalid file.");

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var fileData = ms.ToArray();

        var fileModel = new FileModel
        {
            FileName = file.FileName,
            FileData = fileData,
            FileType = file.ContentType
        };

        var result = await _fileRepository.AddAsync(fileModel);
        await _hubContext.Clients.All.SendAsync("FileUploaded", new
        {
            FileId = result.Id,
            FileName = result.FileName
        });
        return fileModel;
    }
}