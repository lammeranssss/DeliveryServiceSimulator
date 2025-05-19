using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;

namespace DeliveryService.Application.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task<List<Order>> GetAllAsync();
    Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status);
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
}