using DeliveryService.Application.Interfaces;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using DeliveryService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Infrastructure.Repositories;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(int id)
        => await context.Orders.FindAsync(id);

    public async Task<List<Order>> GetAllAsync()
        => await context.Orders.ToListAsync();

    public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatus status)
        => await context.Orders.Where(o => o.Status == status).ToListAsync();

    public async Task AddAsync(Order order)
        => await context.Orders.AddAsync(order);

    public async Task UpdateAsync(Order order)
        => await Task.FromResult(context.Orders.Update(order));
}