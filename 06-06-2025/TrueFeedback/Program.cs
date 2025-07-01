using System.Text;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using TrueFeedback.Contexts;
using TrueFeedback.Hubs;
using TrueFeedback.Interfaces;
using TrueFeedback.Models;
using TrueFeedback.Repositories;
using TrueFeedback.Services;

namespace TrueFeedback;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        
        builder.Services.AddInMemoryRateLimiting();
        builder.Services.AddMemoryCache();

        builder.Services.Configure<IpRateLimitOptions>(options =>
        {
            options.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Period = "10s",
                    Limit = 100
                }
            };
        });

        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new() { Title = "TrueFeedback API", Version = "v1" });

            // Add JWT Auth definition
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "Enter your JWT token like this: **Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...**"
            });

            // Make all endpoints use Bearer auth
            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
        
        var jwtKey = builder.Configuration["Keys:JwtTokenKey"];
        var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
        
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                };
            });
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:4200"); 
            });
        });
        builder.Services.AddScoped<IRepository<Guid, User>, UserRepository>();
        builder.Services.AddScoped<IRepository<Guid, Role>, RoleRepository>();
        builder.Services.AddScoped<IRepository<Guid, Feedback>, FeedbackRepository>();
        
        builder.Services.AddSignalR();

        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<RoleService>();
        builder.Services.AddScoped<FeedbackService>();
        builder.Services.AddScoped<AuthService>();
        
        builder.Services.AddDbContext<TrueFeedbackContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        

        app.UseHttpsRedirection();
        app.UseCors();
        app.UseIpRateLimiting();
        app.MapControllers();
        app.UseAuthentication(); 
        app.UseAuthorization();
        
        app.MapHub<TrueFeedbackHub>("/frontend/listener");


        app.Run();
    }
}