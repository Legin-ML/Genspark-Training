// Services/ProductService.cs
using ShopMigrationAPI.Interfaces.Repositories;
using ShopMigrationAPI.Interfaces.Services;
using ShopMigrationAPI.Models;
using ShopMigrationAPI.Models.DTOs;

namespace ShopMigrationAPI.Services;
public class ProductService 
{
    private readonly IRepository<Product> _productRepo;

    public ProductService(IRepository<Product> productRepo)
    {
        _productRepo = productRepo;
    }

    public IEnumerable<ProductDTO> GetProducts(int page, int pageSize, int? categoryId)
    {
        var query = _productRepo.GetAll();

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.Categoryid == categoryId.Value);
        }

        return query
            .OrderByDescending(p => p.Productid)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductDTO
            {
                Productid = p.Productid,
                Productname = p.Productname,
                Price = p.Price,
                Categoryid = p.Categoryid
            });
    }

    public ProductDTO GetProductById(int id)
    {
        var product = _productRepo.GetById(id);
        if (product == null) return null;

        return new ProductDTO
        {
            Productid = product.Productid,
            Productname = product.Productname,
            Price = product.Price,
            Categoryid = product.Categoryid
        };
    }
}