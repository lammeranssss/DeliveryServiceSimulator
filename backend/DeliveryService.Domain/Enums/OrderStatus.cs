namespace DeliveryService.Domain.Enums;

public enum OrderStatus
{
    Pending,    // Ожидает курьера
    InProgress, // В процессе доставки
    Delivered   // Доставлен
}