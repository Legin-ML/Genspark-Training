using AccuNotify.Models;
using AccuNotify.Models.DTOs;
using AccuNotify.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AccuNotify.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly SecurityService _securityService;

    public UserService(UserRepository userRepository, SecurityService securityService)
    {
        _userRepository = userRepository;
        _securityService = securityService;
    }

    public async Task<User> CreateUserAsync(UserCreateDTO userDto)
    {
        var existingUsers = await _userRepository.GetAllAsync();
        if (existingUsers.Any(u => u.Email.ToLower() == userDto.Email.ToLower()))
            throw new InvalidOperationException("User with this email already exists.");

        var user = new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
            Role = userDto.Role,
            Password = _securityService.HashPassword(userDto.Password)
        };

        return await _userRepository.AddAsync(user);
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var users = await _userRepository.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

        if (user == null)
            throw new UnauthorizedAccessException("Invalid credentials.");

        if (!_securityService.VerifyPassword(password, user.Password))
            throw new UnauthorizedAccessException("Invalid credentials.");

        return _securityService.GenerateJwtToken(user);
    }
}