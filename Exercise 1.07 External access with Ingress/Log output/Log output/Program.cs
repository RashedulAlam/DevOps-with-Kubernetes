using Log_output.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<StatusLoggerBackgroundService>();
builder.Services.AddHostedService(x => x.GetRequiredService<StatusLoggerBackgroundService>());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/current-status", () =>
{
    var guid = Guid.NewGuid().ToString("D");

    return $"{DateTime.UtcNow:s}Z : {guid}";
})
.WithName("CurrentStatus")
.WithOpenApi();

app.Run();
