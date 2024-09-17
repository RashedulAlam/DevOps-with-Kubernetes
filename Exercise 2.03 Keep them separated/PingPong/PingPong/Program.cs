using PingPong;
using PingPong.Services;

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
        // var key = configuration.GetValue<string>(Constants.FilePathKey);
        //
        // if (!Directory.Exists(key))
        // {
        //     Directory.CreateDirectory(key);
        // }
        //
        // var fullPath = Path.Join(key, Constants.FileName);

        var content = $"Ping / Pongs: {pingStorage.GetCount()}";

        // File.WriteAllText(fullPath,content);

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