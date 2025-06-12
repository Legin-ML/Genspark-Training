using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TrueFeedback.Models.DTOs;
using TrueFeedback.Services;

namespace TrueFeedback.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Authenticate user and return JWT & refresh token.
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLoginReqDto userDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var token = await _authService.LoginAsync(userDto);
        return Ok(token);
    }

    /// <summary>
    /// Refresh an expired access token using a valid refresh token.
    /// </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshReqDto refreshDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var token = await _authService.RefreshTokenAsync(refreshDto);
        return Ok(token);
    }

    /// <summary>
    /// Invalidate the current refresh token (logout).
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("Invalid user identity");

        await _authService.LogoutAsync(userId);
        return Ok(new { message = "Successfully logged out." });
    }

    /// <summary>
    /// Get current authenticated user's basic details.
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = User.FindFirstValue(ClaimTypes.Email);
        var role = User.FindFirstValue(ClaimTypes.Role);

        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        return Ok(new
        {
            Id = userId,
            Email = email,
            Role = role
        });
    }
}
