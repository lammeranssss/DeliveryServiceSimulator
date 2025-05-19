namespace DeliveryService.Application.Interfaces;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IOrderRepository Orders { get; }
    IRatingRepository Ratings { get; }
    Task<bool> SaveChangesAsync();
}