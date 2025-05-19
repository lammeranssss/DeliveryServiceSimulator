using Microsoft.EntityFrameworkCore;
using DeliveryService.Infrastructure.Data;
using System;
using DeliveryService.Application.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using DeliveryService.Infrastructure.Services;
using Microsoft.IdentityModel.Tokens;
using DeliveryService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
             .AllowAnyHeader()
             .AllowAnyMethod()
             .AllowCredentials();
    });
});

builder.Services.AddControllers();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// === Конфигурация JWT ===
var jwtSettings = builder.Configuration.GetSection("Jwt"); // Читаем настройки из appsettings.json
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);   // Преобразуем ключ в байты

// Настройка аутентификации
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,                  // Проверять издателя (Issuer)
        ValidateAudience = true,                // Проверять получателя (Audience)
        ValidateLifetime = true,                // Проверять срок действия
        ValidateIssuerSigningKey = true,        // Проверять подпись
        ValidIssuer = jwtSettings["Issuer"],    // Допустимый издатель
        ValidAudience = jwtSettings["Audience"],// Допустимый получатель
        IssuerSigningKey = new SymmetricSecurityKey(key) // Ключ для проверки подписи
    };
});

// Настройка авторизации (для ролей)
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.UseCors("ReactFrontend");

app.MapControllers();

app.Run();
