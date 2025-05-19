using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryService.Domain.Entities;

public class Rating
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Range(1, 5)]
    public int Score { get; set; }

    public string Comment { get; set; }

    [ForeignKey("Order")]
    public int OrderId { get; set; }

    [ForeignKey("Customer")]
    public int CustomerId { get; set; }

    public Order Order { get; set; }
    public User Customer { get; set; }
}