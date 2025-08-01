// Controllers/ProductController.cs
using Microsoft.AspNetCore.Mvc;
using ShopMigrationAPI.Interfaces.Services;
using ShopMigrationAPI.Models.DTOs;
using ShopMigrationAPI.Services;

namespace ShopMigrationAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ProductDTO>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] int? categoryId = null)
    {
        var products = _productService.GetProducts(page, pageSize, categoryId);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public ActionResult<ProductDTO> GetById(int id)
    {
        var product = _productService.GetProductById(id);
        if (product == null)
            return NotFound();
        return Ok(product);
    }
}