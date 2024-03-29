using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ToX.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebUtilities;
using ToX.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddUserSecrets<Program>();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    var connectionStringBuilder = new NpgsqlConnectionStringBuilder();
    var configuration = builder.Configuration;

    connectionStringBuilder.Host = configuration["DB_HOST"];
    connectionStringBuilder.Port = int.Parse(configuration["DB_PORT"]);
    connectionStringBuilder.Database = configuration["DB_NAME"];
    connectionStringBuilder.Username = configuration["DB_USER"];
    connectionStringBuilder.Password = configuration["DB_PASSWORD"];
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
        ValidIssuer = builder.Configuration["JWT_SETTINGS_ISSUER"],
        ValidAudience = builder.Configuration["JWT_SETTINGS_AUDIENCE"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT_SETTINGS_KEY"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
}));
builder.Services.AddAuthorizationCore();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
dbContext.Database.Migrate();

app.MapControllers();
app.MapHub<QuizHub>("quizHub");

app.UseCors(x => x
    .AllowAnyOrigin() //.WithOrigins("http://localhost:4200", "http://172.23.49.21:8017")
    .AllowAnyHeader()
    .AllowAnyMethod());

//app.UseHttpsRedirection();
app.UseWebSockets();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.Run();