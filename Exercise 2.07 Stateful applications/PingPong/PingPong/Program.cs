using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PingPong.Business;
using PingPong.Business.Services;
using PingPong.Infrastructure;
using PingPong.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PingPongContext>(x =>
{
    var connectionStringConfig = builder.Configuration.GetConnectionString("postgres");

    if (!builder.Environment.IsDevelopment())
    {
        connectionStringConfig = Environment.GetEnvironmentVariable("DATABASE_SERVER_URL");

        if (connectionStringConfig != null)
            connectionStringConfig = string.Format(connectionStringConfig,
                Environment.GetEnvironmentVariable("DB_USER"), Environment.GetEnvironmentVariable("DB_PASSWORD"));
    }

    x.UseNpgsql(connectionStringConfig,
        optionsBuilder => optionsBuilder.MigrationsAssembly(Assembly.GetAssembly(typeof(PingPongContext))?.ToString()));
});
builder.Services.RegisterInfrastructureServices();
builder.Services.RegisterBusinessServices();

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
await scope.ServiceProvider.GetRequiredService<PingPongContext>().Database.MigrateAsync();

app.UsePathBase("/pingpong");

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/ping", async ([FromServices]IPingStorage pingStorage, IConfiguration configuration) =>
    {
        var content = $"Ping / Pongs: {await pingStorage.GetCount()}";

        return content;
    })
.WithName("GetPing")
.WithOpenApi();

app.MapPost("/reset-ping", async ([FromServices] IPingStorage pingStorage) =>
    {
        await pingStorage.RestCount();
    })
    .WithName("ResetPing")
    .WithOpenApi();

app.Run();