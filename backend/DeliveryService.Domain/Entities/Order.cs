using DeliveryService.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryService.Domain.Entities;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DeliveredAt { get; set; }

    [Required]
    public OrderStatus Status { get; set; } = OrderStatus.Pending; 

    // Внешние ключи
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }

    [ForeignKey("Courier")]
    public int? CourierId { get; set; }

    // Навигационные свойства
    public User Customer { get; set; }
    public User Courier { get; set; }
    public Rating Rating { get; set; }
}