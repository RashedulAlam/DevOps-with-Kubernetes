using PingPong.Business.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IPingStorage, PingStorage>();

var app = builder.Build();

app.UsePathBase("/pingpong");

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/ping", (IPingStorage pingStorage, IConfiguration configuration) =>
    {
        var content = $"Ping / Pongs: {pingStorage.GetCount()}";

        return content;
    })
.WithName("GetPing")
.WithOpenApi();

app.MapPost("/reset-ping", (IPingStorage pingStorage) =>
    {
        pingStorage.RestCount();
    })
    .WithName("ResetPing")
    .WithOpenApi();

app.Run();