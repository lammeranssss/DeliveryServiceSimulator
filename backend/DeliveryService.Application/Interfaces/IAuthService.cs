using DeliveryService.Domain.Entities;

namespace DeliveryService.Application.Interfaces;

public interface IAuthService
{
    Task<string> GenerateTokenAsync(User user); 
    Task<User?> AuthenticateAsync(string email, string password); 
}