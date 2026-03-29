using System.Text;
using BankingLite.Api.Data;
using BankingLite.Api.Interface;
using BankingLite.Api.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework Core with In-Memory Database
builder.Services.AddDbContext<BankingLiteDbContext>(options =>
    options.UseInMemoryDatabase("BankingLiteDb"));

// Register repositories
builder.Services.AddScoped<IUserRepo, UserRepo>();

// JWT Authentication configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings.GetValue<string>("Key");
if (string.IsNullOrEmpty(jwtKey))
    throw new InvalidOperationException("Jwt:Key is not configured. Add it in appsettings.json.");

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

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
        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
        ValidAudience = jwtSettings.GetValue<string>("Audience"),
        IssuerSigningKey = key
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();

app.Run();
