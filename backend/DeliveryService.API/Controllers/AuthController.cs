using DeliveryService.Application.Interfaces;
using DeliveryService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _authService.AuthenticateAsync(request.Email, request.Password);
        if (user == null)
            return Unauthorized("Неверный email или пароль");

        var token = await _authService.GenerateTokenAsync(user);


        var roleString = user.Role.ToString() ?? "Unknown";

        return Ok(new
        {
            token,               
            role = user.Role.ToString()  
        });
    }

    public record LoginRequest(string Email, string Password);
}
