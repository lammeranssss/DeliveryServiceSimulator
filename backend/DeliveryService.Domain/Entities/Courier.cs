using DeliveryService.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Domain.Entities;

public class Courier
{
    public int Id { get; set; }

    [Required]
    public VehicleType Vehicle { get; set; } 

    [Required]
    public string Location { get; set; }

    // Связь с User
    public int UserId { get; set; }
    public User User { get; set; }
}