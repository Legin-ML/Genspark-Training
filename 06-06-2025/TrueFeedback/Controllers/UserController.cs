using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TrueFeedback.Models;
using TrueFeedback.Services;

namespace TrueFeedback.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    /// <summary>List users (with optional pagination, search, and sorting)</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<User>), 200)]
    public async Task<IActionResult> GetUsers([FromQuery] QueryParameters query)
    {
        var users = await _userService.GetAllAsync(query);
        return Ok(users);
    }

    /// <summary>Get user by ID</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(User), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
        return Ok(user);
    }

    /// <summary>Create a new user</summary>
    [HttpPost]
    [ProducesResponseType(typeof(User), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        try
        {
            var createdUser = await _userService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>Update user details</summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(User), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserDto dto)
    {
        var updatedUser = await _userService.UpdateAsync(id, dto);
        return Ok(updatedUser);
    }

    /// <summary>Delete a user by ID</summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }
}
