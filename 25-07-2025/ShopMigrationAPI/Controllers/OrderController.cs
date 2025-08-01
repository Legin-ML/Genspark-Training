using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using ShopMigrationAPI.Models;
using ShopMigrationAPI.Services;

namespace ShopMigrationAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Order>> GetOrders([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
    {
        try
        {
            var orders = _orderService.GetOrdersPaged(page, pageSize);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public ActionResult<Order> GetOrderById(int id)
    {
        try
        {
            var order = _orderService.GetOrderById(id);
            return Ok(order);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    public ActionResult CreateOrder([FromBody] Order order)
    {
        try
        {
            _orderService.CreateOrder(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Orderid }, order);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public ActionResult UpdateOrder(int id, [FromBody] Order order)
    {
        try
        {
            if (id != order.Orderid)
            {
                return BadRequest("Order ID mismatch.");
            }

            _orderService.UpdateOrder(order);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteOrder(int id)
    {
        try
        {
            _orderService.DeleteOrder(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /*public ActionResult ExportOrderListing()
    {
        try
        {
            // TODO: Fix
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }*/
    
    

}