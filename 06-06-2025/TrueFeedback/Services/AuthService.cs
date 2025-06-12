using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using TrueFeedback.Interfaces;
using TrueFeedback.Models;
using TrueFeedback.Models.DTOs;
using TrueFeedback.Security;

namespace TrueFeedback.Services;

public class AuthService
{
    private readonly IRepository<Guid, User> _userRepository;
    private readonly IRepository<Guid, Role> _roleRepository;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IConfiguration _config;

    public AuthService(
        IRepository<Guid, User> userRepository,
        ILogger<AuthenticationService> logger,
        IRepository<Guid, Role> roleRepository,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _logger = logger;
        _roleRepository = roleRepository;
        _config = configuration;
    }

    public async Task<AuthRespDto> LoginAsync(UserLoginReqDto userDto)
    {
        try
        {
            var user = await FindUserByEmailAsync(userDto.Email);
            if (user == null)
            {
                _logger.LogWarning("Login attempt failed: no user found with email {Email}", userDto.Email);
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            if (string.IsNullOrEmpty(userDto.Password) || string.IsNullOrEmpty(user.Password))
            {
                _logger.LogWarning("Login failed for {Email} due to missing password or stored hash", userDto.Email);
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            bool passwordValid = PasswordUtils.VerifyPassword(userDto.Password, user.Password);
            if (!passwordValid)
            {
                _logger.LogWarning("Incorrect password provided for user {Email}", userDto.Email);
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "User")
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Keys:JwtTokenKey"]));
            var signingCreds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: signingCreds
            );

            var newRefreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateAsync(user.Id, user);

            _logger.LogInformation("User {Email} successfully authenticated", userDto.Email);

            return new AuthRespDto()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = newRefreshToken,
                Expires= jwtToken.ValidTo,
            };
        }
        catch (UnauthorizedAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during login for {Email}", userDto.Email);
            throw new Exception("Login process encountered an issue, please try again.");
        }
    }

    private async Task<User> FindUserByEmailAsync(string userDtoEmail)
    {
        if (string.IsNullOrWhiteSpace(userDtoEmail))
            throw new ArgumentException("Email must not be empty.", nameof(userDtoEmail));

        var users = await _userRepository.GetAllAsync();
        var user = users.FirstOrDefault(u => 
            u.Email.Equals(userDtoEmail, StringComparison.OrdinalIgnoreCase));

        if (user == null)
            throw new KeyNotFoundException($"No user found with email: {userDtoEmail}");
        
        if (user.Role == null)
        {
            var roles = await _roleRepository.GetAllAsync();
            user.Role = roles.FirstOrDefault(r => r.Id == user.RoleId);
        }

        return user;
    }

    public async Task<AuthRespDto> RefreshTokenAsync(RefreshReqDto refreshDto)
    {

            if (refreshDto == null || string.IsNullOrWhiteSpace(refreshDto.RefreshToken))
            {
                _logger.LogWarning("Refresh token request missing required data");
                throw new UnauthorizedAccessException("Invalid token refresh request");
            }

            var user = await _userRepository.GetAsync(refreshDto.UserId);
            if (user == null)
            {
                _logger.LogWarning("Refresh token failed - user with ID {UserId} does not exist", refreshDto.UserId);
                throw new UnauthorizedAccessException("User not found");
            }

            if (user.RefreshToken != refreshDto.RefreshToken)
            {
                _logger.LogWarning("Refresh token mismatch for user ID {UserId}", user.Id);
                _logger.LogDebug("Stored token: {StoredToken} | Provided token: {ProvidedToken}", user.RefreshToken, refreshDto.RefreshToken);
                throw new UnauthorizedAccessException("Refresh token does not match");
            }

            if (!user.RefreshTokenExpiryTime.HasValue || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                _logger.LogWarning("Refresh token expired for user ID {UserId}", user.Id);
                _logger.LogDebug("Expiration: {ExpiryTime} | Now: {CurrentTime}", user.RefreshTokenExpiryTime, DateTime.UtcNow);
                throw new UnauthorizedAccessException("Refresh token expired");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Keys:JwtTokenKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var newJwtToken = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var freshRefreshToken = Guid.NewGuid().ToString();
            user.RefreshToken = freshRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateAsync(user.Id, user);

            _logger.LogInformation("Issued new JWT and refresh token for user ID {UserId}", user.Id);

            return new AuthRespDto()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(newJwtToken),
                RefreshToken = freshRefreshToken, 
                Expires= newJwtToken.ValidTo,
            };
        }


    public async Task LogoutAsync(string userId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                _logger.LogWarning("Logout request missing user ID");
                throw new ArgumentException("User ID must be provided");
            }

            var user = await _userRepository.GetAsync(Guid.Parse(userId));
            if (user == null)
            {
                _logger.LogWarning("Logout failed: no user found with ID {UserId}", userId);
                throw new UnauthorizedAccessException("User not found");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userRepository.UpdateAsync(user.Id, user);

            _logger.LogInformation("User ID {UserId} logged out and tokens invalidated", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout for user ID {UserId}", userId);
            throw new Exception("Logout failed. Please try again.");
        }
    }
}