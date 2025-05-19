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
        // 1. ��������� �����/������
        var user = await authService.AuthenticateAsync(request.Email, request.Password);
        if (user == null)
            return Unauthorized("�������� email ��� ������");

        // 2. ���������� �����
        var token = await authService.GenerateTokenAsync(user);
        return Ok(new { Token = token }); // ���������� ����� �������
    }

    // DTO ��� ������� ������
    public record LoginRequest(string Email, string Password);
}