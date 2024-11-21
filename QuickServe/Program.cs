using QuickServe.Services.Interfaces;
using QuickServe.Services.Implementations;
using QuickServe.Repositories.Interfaces;
using QuickServe.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using QuickServe.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register the DbContext with SQL Server connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register IUserService and its implementation UserService
builder.Services.AddScoped<IUserService, UserService>();  // Ensure this is here!

// Register IUserRepository and UserRepository
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
