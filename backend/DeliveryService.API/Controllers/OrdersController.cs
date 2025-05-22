using DeliveryService.Application.Interfaces;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace DeliveryService.API.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await unitOfWork.Orders.GetAllAsync();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)  
    {
        var order = await unitOfWork.Orders.GetByIdAsync(id);
        return order == null ? NotFound() : Ok(order);
    }

    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateRequest request)
    {
        var order = new Order
        {
            Address = request.Address,
            CustomerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await unitOfWork.Orders.AddAsync(order);
        await unitOfWork.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
    }

    [HttpPatch("{id}/accept")]
    [Authorize(Roles = "Courier")]
    public async Task<IActionResult> AcceptOrder(int id)
    {
        var order = await unitOfWork.Orders.GetByIdAsync(id);
        if (order == null) return NotFound();

        order.Status = OrderStatus.InProgress;
        order.CourierId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        await unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}/complete")]
    [Authorize(Roles = "Courier")]
    public async Task<IActionResult> CompleteOrder(int id)
    {
        var order = await unitOfWork.Orders.GetByIdAsync(id);
        if (order == null || order.CourierId.ToString() != User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
            return Forbid();

        order.Status = OrderStatus.Delivered;
        order.DeliveredAt = DateTime.UtcNow;

        await unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    // DTO для запросов
    public record OrderCreateRequest(string Address);
}