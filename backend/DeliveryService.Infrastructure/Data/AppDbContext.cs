using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSets для всех сущностей
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Courier> Couriers { get; set; }
    public DbSet<Rating> Ratings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Конвертеры enum в строку для читаемости в БД
        modelBuilder
            .Entity<Order>()
            .Property(o => o.Status)
            .HasConversion<string>();

        modelBuilder
            .Entity<User>()
            .Property(u => u.Role)
            .HasConversion<string>(); 

        modelBuilder
            .Entity<Courier>()
            .Property(c => c.Vehicle)
            .HasConversion<string>(); 

        // Настройка связей
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Courier)
            .WithMany()
            .HasForeignKey(o => o.CourierId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Courier>()
            .HasOne(c => c.User)
            .WithOne()
            .HasForeignKey<Courier>(c => c.UserId);
    }
}