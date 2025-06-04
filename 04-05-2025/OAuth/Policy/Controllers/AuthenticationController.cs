using FirstAPI.Interfaces;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly FirstAPI.Interfaces.IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(
            FirstAPI.Interfaces.IAuthenticationService authenticationService,
            ITokenService tokenService,
            ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserLoginResponse>> UserLogin(UserLoginRequest loginRequest)
        {
            try
            {
                var result = await _authenticationService.Login(loginRequest);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Login failed.");
                return Unauthorized(e.Message);
            }
        }

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var redirectUrl = Url.Action(nameof(GoogleCallback), "Authentication");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!result.Succeeded || result.Principal == null)
                return Unauthorized("Google authentication failed");

            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
                return BadRequest("Email not received from Google");

            return Ok(new
            {
                name,
                email
            });
        }
    }
}
