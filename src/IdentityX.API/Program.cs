using System.Text;
using IdentityX.API.Endpoints;
using IdentityX.Core.Interfaces;
using IdentityX.Infrastructure.Data;
using IdentityX.Infrastructure.Repositories;
using IdentityX.Services.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = """
                      JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      """,
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!,
        b => b.MigrationsAssembly("TesTestTrek.API")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Add Serilog configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Read settings from appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console() // Logs to console
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) // Logs to a file
    .CreateLogger();

// Replace default logger
builder.Host.UseSerilog();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapAuthEndpoints();
app.MapUsersEndpoints();
app.UseSerilogRequestLogging();

app.Run();