// Controllers/OrderController.cs
using Microsoft.AspNetCore.Mvc;
using ShopMigrationAPI.Interfaces.Services;
using ShopMigrationAPI.Models.DTOs;
using ShopMigrationAPI.Services;

namespace ShopMigrationAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ShoppingCartController : ControllerBase
{
    private readonly OrderService _orderService;

    public ShoppingCartController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("checkout")]
    public IActionResult Checkout([FromBody] OrderRequestDTO orderDto)
    {
        if (orderDto == null || orderDto.Items == null || !orderDto.Items.Any())
        {
            return BadRequest("Invalid order data.");
        }

        try
        {
            _orderService.ProcessOrder(orderDto);
            return Ok("Order placed successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error processing order: {ex.Message}");
        }
    }
}