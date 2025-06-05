using FileHandleAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FileHandleAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileHandlerController : ControllerBase
{
    private readonly IFileHandlerService _fileHandlerService;

    public FileHandlerController(IFileHandlerService fileHandlerService)
    {
        _fileHandlerService = fileHandlerService;
    }

    [HttpGet("download")]
    public async Task<IActionResult> GetFile([FromQuery] string fileName)
    {
        try
        {
            var fileData = await _fileHandlerService.GetFile(fileName);
            return File(fileData, "application/octet-stream", fileName);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("upload")]
    public async Task<IActionResult> PostFile(IFormFile file)
    {
        try
        {
            await _fileHandlerService.UploadFile(file);
            return Ok("File uploaded successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpGet("display")]
    public async Task<IActionResult> DisplayFileContent([FromQuery] string fileName)
    {
        try
        {
            var fileBytes = await _fileHandlerService.GetFile(fileName);
            var fileContent = System.Text.Encoding.UTF8.GetString(fileBytes);
            return Ok(fileContent);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}