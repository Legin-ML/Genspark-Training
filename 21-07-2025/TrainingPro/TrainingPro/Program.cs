using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using TrainingPro.Contexts;
using TrainingPro.Repositories;
using TrainingPro.Services;

namespace TrainingPro;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Load configuration
        var config = builder.Configuration;
        
        builder.Services.AddDbContext<TrainingDBContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        builder.Services.AddSingleton(x =>
        {
            var blobConnection = config["AzureBlobStorage:ConnectionString"];
            return new BlobServiceClient(blobConnection);
        });
        
        builder.Services.AddScoped<VideoService>();
        builder.Services.AddScoped<VideoRepository>();
        
        builder.Services.AddControllers();
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        // Swagger/OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Swagger only in development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseCors("AllowAll");

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}