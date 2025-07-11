using BlobStorage.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            using var stream = file.OpenReadStream();
            await _fileService.UploadFile(stream, file.FileName);

            return Ok("File uploaded successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("download")]
    public async Task<IActionResult> Download([FromQuery] string fileName)
    {
        try
        {
            var fileStream = await _fileService.GetFile(fileName);
            return File(fileStream, "application/octet-stream", fileName);
        }
        catch (FileNotFoundException)
        {
            return NotFound("File not found");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("display")]
    public async Task<IActionResult> Display([FromQuery] string fileName)
    {
        try
        {
            var fileStream = await _fileService.GetFile(fileName);

            using var reader = new StreamReader(fileStream);
            var content = await reader.ReadToEndAsync();

            return Ok(content);
        }
        catch (FileNotFoundException)
        {
            return NotFound("File not found");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}