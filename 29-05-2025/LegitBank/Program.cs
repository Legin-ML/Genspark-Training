using LegitBank.Contexts;
using LegitBank.Interfaces;
using LegitBank.Models;
using LegitBank.Repositories;
using LegitBank.Services;
using Microsoft.EntityFrameworkCore;

namespace LegitBank;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddControllers().AddJsonOptions(opts =>
        {
            opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            opts.JsonSerializerOptions.WriteIndented = true;
        });
        
        builder.Services.AddTransient<IRepository<int, Account>, AccountRepository>();
        builder.Services.AddTransient<IRepository<int, Transaction>, TransactionRepository>();
        
        
        builder.Services.AddScoped<ITransactionService, TransactionService>();

        
        builder.Services.AddScoped<IAccountService, AccountService>();
        
        builder.Services.AddDbContext<LegitBankContext>(opts =>
        {
            opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();
        
        app.MapControllers();
        app.Run();
    }
}