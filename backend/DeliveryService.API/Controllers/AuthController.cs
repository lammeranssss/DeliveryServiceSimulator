using DeliveryService.Application.Interfaces;
using DeliveryService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // 1. Проверяем логин/пароль
        var user = await authService.AuthenticateAsync(request.Email, request.Password);
        if (user == null)
            return Unauthorized("Неверный email или пароль");

        // 2. Генерируем токен
        var token = await authService.GenerateTokenAsync(user);
        return Ok(new { Token = token }); // Возвращаем токен клиенту
    }

    // DTO для запроса логина
    public record LoginRequest(string Email, string Password);
}