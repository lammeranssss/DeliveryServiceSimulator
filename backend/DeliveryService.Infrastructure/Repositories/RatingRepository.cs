using DeliveryService.Application.Interfaces;
using DeliveryService.Domain.Entities;
using DeliveryService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Infrastructure.Repositories;

public class RatingRepository(AppDbContext context) : IRatingRepository
{
    public async Task AddAsync(Rating rating)
        => await context.Ratings.AddAsync(rating);

    public async Task<Rating?> GetByOrderIdAsync(int orderId)
        => await context.Ratings.FirstOrDefaultAsync(r => r.OrderId == orderId);
}