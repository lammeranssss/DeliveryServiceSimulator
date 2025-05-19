using DeliveryService.Domain.Entities;

namespace DeliveryService.Application.Interfaces;

public interface IRatingRepository
{
    Task AddAsync(Rating rating);
    Task<Rating?> GetByOrderIdAsync(int orderId);
}