using Microsoft.EntityFrameworkCore;
using TrueFeedback.Interfaces;
using TrueFeedback.Models;
using TrueFeedback.Repositories;
using TrueFeedback.Security;

namespace TrueFeedback.Services;

public class UserService
{
    private readonly IRepository<Guid, User> _userRepository;
    private readonly IRepository<Guid, Role> _roleRepository;

    public UserService(IRepository<Guid, User> userRepository, IRepository<Guid, Role> roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<User>> GetAllAsync(QueryParameters query)
    {
        var users = (await _userRepository.GetAllAsync())
            .Where(u => !u.IsDeleted)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            users = users.Where(u =>
                u.Email.Contains(query.Search) ||
                u.UserName.Contains(query.Search));
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            users = query.SortBy.ToLower() switch
            {
                "username" => query.SortDescending ? users.OrderByDescending(u => u.UserName) : users.OrderBy(u => u.UserName),
                "email" => query.SortDescending ? users.OrderByDescending(u => u.Email) : users.OrderBy(u => u.Email),
                _ => users
            };
        }

        return users
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetAsync(id);
        if (user.IsDeleted)
        {
            throw new KeyNotFoundException($"User with id {id} not found");
        }
        return user;
    }

    public async Task<User> CreateAsync(CreateUserDto dto)
    {
        var roles = await _roleRepository.GetAllAsync();
        var role = roles.FirstOrDefault(r => r.RoleName.Equals(dto.RoleName, StringComparison.OrdinalIgnoreCase));
        if (role == null)
        {
            throw new ArgumentException($"Role '{dto.RoleName}' not found");
        }

        var newUser = new User
        {
            Id = Guid.NewGuid(),
            UserName = dto.UserName,
            Email = dto.Email,
            Password = PasswordUtils.HashPassword(dto.Password),
            RoleId = role.Id,
            IsDeleted = false,
            Created = DateTime.UtcNow
        };

        return await _userRepository.AddAsync(newUser);
    }

    public async Task<User> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _userRepository.GetAsync(id);

        if (user.IsDeleted)
        {
            throw new KeyNotFoundException($"User with id {id} not found");
        }

        var roles = await _roleRepository.GetAllAsync();
        var role = roles.FirstOrDefault(r => r.RoleName.Equals(dto.RoleName, StringComparison.OrdinalIgnoreCase));
        if (role == null)
        {
            throw new ArgumentException($"Role '{dto.RoleName}' not found");
        }

        user.Email = dto.Email;
        user.UserName = dto.UserName;
        user.RoleId = role.Id;

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            user.Password = PasswordUtils.HashPassword(dto.Password);
        }

        return await _userRepository.UpdateAsync(id, user);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _userRepository.GetAsync(id);
        user.IsDeleted = true;
        await _userRepository.UpdateAsync(id, user);
        return true;
    }
}
