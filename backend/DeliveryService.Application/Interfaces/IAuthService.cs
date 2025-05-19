using DeliveryService.Domain.Entities;

namespace DeliveryService.Application.Interfaces;

public interface IAuthService
{
    Task<string> GenerateTokenAsync(User user); // Генерация токена для пользователя
    Task<User?> AuthenticateAsync(string email, string password); // Проверка логина/пароля
}