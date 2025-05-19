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

// === ������������ JWT ===
var jwtSettings = builder.Configuration.GetSection("Jwt"); // ������ ��������� �� appsettings.json
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);   // ����������� ���� � �����

// ��������� ��������������
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,                  // ��������� �������� (Issuer)
        ValidateAudience = true,                // ��������� ���������� (Audience)
        ValidateLifetime = true,                // ��������� ���� ��������
        ValidateIssuerSigningKey = true,        // ��������� �������
        ValidIssuer = jwtSettings["Issuer"],    // ���������� ��������
        ValidAudience = jwtSettings["Audience"],// ���������� ����������
        IssuerSigningKey = new SymmetricSecurityKey(key) // ���� ��� �������� �������
    };
});

// ��������� ����������� (��� �����)
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.UseCors("ReactFrontend");

app.MapControllers();

app.Run();
