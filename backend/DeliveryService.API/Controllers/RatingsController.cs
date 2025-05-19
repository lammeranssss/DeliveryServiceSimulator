using DeliveryService.Application.Interfaces;
using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeliveryService.API.Controllers;

[ApiController]
[Route("api/ratings")]
[Authorize(Roles = "Customer")] // ������ ������� ����� ���������
public class RatingsController(IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateRating([FromBody] RatingCreateRequest request)
    {
        var order = await unitOfWork.Orders.GetByIdAsync(request.OrderId);
        if (order == null || order.Status != OrderStatus.Delivered)
            return BadRequest("����� �� ������ ��� ��� �� ���������");

        var rating = new Rating
        {
            OrderId = request.OrderId,
            CustomerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
            Score = request.Score,
            Comment = request.Comment
        };

        await unitOfWork.Ratings.AddAsync(rating);
        await unitOfWork.SaveChangesAsync();

        return Ok(rating);
    }

    // DTO ��� ��������
    public record RatingCreateRequest(
        int OrderId,
        int Score,
        string? Comment);
}