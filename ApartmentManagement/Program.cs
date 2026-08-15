using ApartmentManagement.Data;
using ApartmentManagement.Repositories;
using ApartmentManagement.Repositories.Impl;
using ApartmentManagement.Services;
using ApartmentManagement.Services.Impl;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Đăng ký ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký Repository
builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();
builder.Services.AddScoped<IApartmentRepository, ApartmentRepository>();
builder.Services.AddScoped<ITenantRepository, TenantRepository>();

// Đăng ký Service
builder.Services.AddScoped<IBuildingService, BuildingService>();
builder.Services.AddScoped<IApartmentService, ApartmentService>();
builder.Services.AddScoped<ITenantService, TenantService>();

// Đăng ký Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấp quyền cho Frontend gọi API (CORS)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()  // Cho phép mọi cổng (localhost:3000, 5173...)
              .AllowAnyHeader()  // Cho phép gửi mọi loại dữ liệu
              .AllowAnyMethod(); // Cho phép mọi lệnh (GET, POST, PUT, DELETE)
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

if (app.Environment.IsDevelopment())
{
    // Bật giao diện Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Kích hoạt CORS
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
