using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using ToX.Models;
using dotenv.net;

DotEnv.Fluent()
    .WithExceptions()
    .WithEnvFiles(Path.Combine("..", ".env"))
    .WithTrimValues()
    .WithDefaultEncoding()
    .WithoutOverwriteExistingVars()
    .WithoutProbeForEnv()
    .Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    var connectionStringBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = Environment.GetEnvironmentVariable("DB_HOST"),
        Port = int.Parse(Environment.GetEnvironmentVariable("DB_PORT")),
        Database = Environment.GetEnvironmentVariable("DB_NAME"),
        Username = Environment.GetEnvironmentVariable("DB_USER"),
        Password = Environment.GetEnvironmentVariable("DB_PASSWORD")
    };

    options.UseNpgsql(connectionStringBuilder.ConnectionString);

    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.EnableSensitiveDataLogging();
    options.EnableDetailedErrors();
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer((x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_SETTINGS_ISSUER"),
        ValidAudience = Environment.GetEnvironmentVariable("JWT_SETTINGS_AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SETTINGS_KEY"))),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
}));
builder.Services.AddAuthorizationCore();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(x => x
    .WithOrigins("http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
dbContext.Database.Migrate();

app.MapControllers();

app.Run();