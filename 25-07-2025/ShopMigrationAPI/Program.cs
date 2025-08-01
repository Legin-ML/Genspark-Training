using Microsoft.EntityFrameworkCore;
using ShopMigrationAPI.Interfaces.Repositories;
using ShopMigrationAPI.Interfaces.Services;
using ShopMigrationAPI.Mapper;
using ShopMigrationAPI.Models;
using ShopMigrationAPI.Repositories;
using ShopMigrationAPI.Services;

namespace ShopMigrationAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<ShopMigrationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddControllers();

        builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));
        
        // Repositories
        builder.Services.AddScoped<IRepository<Color>, ColorRepository>();
        builder.Services.AddScoped<IRepository<Category>, CategoryRepository>();
        builder.Services.AddScoped<IRepository<Order>, OrderRepository>();
                // builder.Services.AddScoped<IRepository<News>, NewsRepository>();
        builder.Services.AddScoped<NewsRepository>();
        builder.Services.AddScoped<IRepository<Product>, ProductRepository>();
        
        // Services
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IColorService, ColorService>();
        builder.Services.AddScoped<OrderService>();
        builder.Services.AddScoped<NewsManagementService>();
        builder.Services.AddScoped<ProductService>();

        // Add authorization and configure Swagger
        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Build the application
        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Map the API endpoints
        app.MapControllers();

        // Run the app
        app.Run();
    }
}