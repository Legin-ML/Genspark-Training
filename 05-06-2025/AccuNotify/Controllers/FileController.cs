using AccuNotify.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccuNotify.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly FileService _fileService;

    public FileController(FileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    [Authorize(Roles = "HRAdmin")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        try
        {
            var savedFile = await _fileService.UploadFileAsync(file);
            return Ok(new { savedFile.Id, savedFile.FileName, savedFile.FileType });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetFile(int id)
    {
        try
        {
            var file = await _fileService.GetFileAsync(id);
            return File(file.FileData, file.FileType, file.FileName);
        }
        catch (FileNotFoundException)
        {
            return NotFound($"File with ID {id} not found.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}