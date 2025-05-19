using DeliveryService.Application.Interfaces;
using DeliveryService.Infrastructure.Repositories;

namespace DeliveryService.Infrastructure.Data;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public IUserRepository Users { get; } = new UserRepository(context);
    public IOrderRepository Orders { get; } = new OrderRepository(context);
    public IRatingRepository Ratings { get; } = new RatingRepository(context);

    public async Task<bool> SaveChangesAsync()
        => await context.SaveChangesAsync() > 0;
}