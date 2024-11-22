using QuickServe.Repositories.Interfaces;
using QuickServe.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using QuickServe.Data;
using QuickServe.Services.Interfaces;
using QuickServe.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register the DbContext with SQL Server connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Register IUserRepository and UserRepository
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register IUserService and UserService
builder.Services.AddScoped<IUserService, UserService>();

// Register ITokenService and TokenService
builder.Services.AddScoped<ITokenService, TokenService>();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Authentication Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Change this to true for production to require HTTPS
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],   // The issuer from appsettings.json
            ValidAudience = builder.Configuration["Jwt:Audience"], // The audience from appsettings.json
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // The secret key from appsettings.json
        };
    });

// Add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});

var app = builder.Build();

// Enable Swagger UI in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Authentication and Authorization middlewares
app.UseAuthentication();  // Make sure authentication happens before authorization
app.UseAuthorization();   // Ensure authorization happens after authentication

app.MapControllers();

app.Run();
