using Eventra.DTOs;
using Eventra.Services;
using Eventra.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eventra.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDTO dto)
    {
        try
        {
            var user = await _userService.RegisterAsync(dto);
            
            // return UserResponseDTO for security without password hash
            var userResponse = new UserResponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType
            };
            
            return StatusCode(201, userResponse);
        }
        catch (InvalidOperationException ex) // Email already exists
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        try
        {
            var authResponse = await _userService.LoginAsync(dto);
            return Ok(authResponse);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var requestingUserId = User.GetUserId();
        var isAdmin = User.IsAdmin();

        // User can only view their own profile unless they're an Admin
        if (requestingUserId != id && !isAdmin)
            return Forbid();

        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return NotFound(new { message = $"User with ID {id} not found." });

        var userResponse = new UserResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            UserType = user.UserType
        };

        return Ok(userResponse);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        var userResponses = users.Select(user => new UserResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            UserType = user.UserType
        });
        return Ok(userResponses);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDTO dto)
    {
        var requestingUserId = User.GetUserId();
        var isAdmin = User.IsAdmin();

        // User can only update their own profile unless they're an Admin
        if (requestingUserId != id && !isAdmin)
            return Forbid(); // 403 error

        try
        {
            await _userService.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var requestingUserId = User.GetUserId();
        var isAdmin = User.IsAdmin();

        // User can only delete their own account unless they're an Admin
        if (requestingUserId != id && !isAdmin)
            return Forbid();

        try
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}