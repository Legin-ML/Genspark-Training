using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using TrainingPro.Models;
using TrainingPro.Models.DTOs;
using TrainingPro.Repositories;
using TrainingPro.Services;

namespace TrainingPro.Controllers;

[ApiController]
[Route("api/videos")]
public class VideoController : ControllerBase
{
    private readonly VideoService _videoService;
    private readonly VideoRepository _videoRepository;

    public VideoController(VideoService videoService, VideoRepository videoRepository)
    {
        _videoService = videoService;
        _videoRepository = videoRepository;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadVideo([FromForm] VideoUpload dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("No video file uploaded.");

        var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";

        using var stream = dto.File.OpenReadStream();
        await _videoService.UploadFileAsync(stream, fileName);

        var video = new TrainingVideo
        {
            Title = dto.Title,
            Description = dto.Description,
            UploadDate = DateTime.UtcNow,
            BlobFileUri = fileName
        };

        await _videoRepository.AddAsync(video);
        return Ok(video);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var videos = await _videoRepository.GetAllAsync();
        return Ok(videos);
    }

    [HttpGet("{id}/stream")]
    public async Task<IActionResult> Stream(int id)
    {
        var video = await _videoRepository.GetAsync(id);
        var stream = await _videoService.GetFileStreamAsync(video.BlobFileUri);

        return File(stream, "video/mp4", enableRangeProcessing: true);
    }
}
