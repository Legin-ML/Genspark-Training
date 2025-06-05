using AccuNotify.Models.DTOs;
using AccuNotify.Services;
using Microsoft.AspNetCore.Mvc;

namespace AccuNotify.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserCreateDTO dto)
    {
        try
        {
            await _userService.CreateUserAsync(dto);
            return Ok("User registered successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        try
        {
            var token = await _userService.LoginAsync(dto.Email, dto.Password);
            return Ok(new { token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid credentials");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
