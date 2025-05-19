using DeliveryService.Application.Interfaces;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace DeliveryService.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "Admin")] // Только админ!
public class UsersController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await unitOfWork.Users.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await unitOfWork.Users.GetByIdAsync(id);
        return user == null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateRequest request)
    {
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password), // Хешируем пароль
            Role = request.Role
        };

        await unitOfWork.Users.AddAsync(user);
        await unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateRequest request)
    {
        var user = await unitOfWork.Users.GetByIdAsync(id);
        if (user == null) return NotFound();

        user.FirstName = request.FirstName ?? user.FirstName;
        user.LastName = request.LastName ?? user.LastName;
        user.Email = request.Email ?? user.Email;

        await unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await unitOfWork.Users.GetByIdAsync(id);
        if (user == null) return NotFound();

        await unitOfWork.Users.DeleteAsync(user);
        return NoContent();
    }

    // DTO для запросов
    public record UserCreateRequest(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        UserRole Role);

    public record UserUpdateRequest(
        string? FirstName,
        string? LastName,
        string? Email);
}