using DeliveryService.Application.Interfaces;
using DeliveryService.Domain.Entities;
using DeliveryService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(int id)
        => await context.Users.FindAsync(id);

    public async Task<List<User>> GetAllAsync()
        => await context.Users.ToListAsync();

    public async Task AddAsync(User user)
        => await context.Users.AddAsync(user);

    public async Task UpdateAsync(User user)
        => await Task.FromResult(context.Users.Update(user));

    public async Task DeleteAsync(User user)
    {
        context.Users.Remove(user);
        await context.SaveChangesAsync();  
    }

    public async Task<User?> GetByEmailAsync(string email)
        => await context.Users.FirstOrDefaultAsync(u => u.Email == email);
}