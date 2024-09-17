using Microsoft.AspNetCore.Mvc;
using TodoService;
using TodoService.Model;
using TodoService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ITodoStorage, TodoStorage>();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UsePathBase("/todolist-service");
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(x =>
{
    x.AllowAnyHeader();
    x.AllowAnyMethod();
    x.AllowAnyOrigin();
});

app.MapGet("/todos", (ITodoStorage todoStorage) => todoStorage.GetAll())
.WithName("GetTodos")
.WithOpenApi();

app.MapPost("/todos", ([FromBody] Todo todo, ITodoStorage todoStorage) =>
    { 
        var newTodo = todoStorage.Create(todo); 

        return newTodo;
})
.WithName("CreateTodos")
.WithOpenApi();


app.MapGet("/find-image", async ([FromServices] HttpClient httpClient, IConfiguration configuration) =>
{
    var fileDirectory = configuration.GetValue<string>(Constants.FilePathConfigurationKey) ?? string.Empty;
    var cachedFileName = $"{Guid.NewGuid():N}.jpg";
    var fullPath = Path.Combine(fileDirectory, cachedFileName);
    var logFilePath =
        Path.Join(configuration.GetValue<string>(Constants.FilePathConfigurationKey), Constants.LogFileName);

    var shouldDownload = true;

    if (File.Exists(logFilePath))
    {
        var content = await File.ReadAllTextAsync(logFilePath);

        var cachedTimeString = content.Split(Constants.SplitOperator)[0];

        var cachedTime = DateTime.Parse(cachedTimeString);


        if (DateTime.UtcNow.Subtract(cachedTime).TotalMinutes < 60)
        {
            shouldDownload = false;
            cachedFileName = content.Split(Constants.SplitOperator)[1];
        }
    }

    if (shouldDownload)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, Constants.ImageUrl);
        await using Stream contentStream = await (await httpClient.SendAsync(request)).Content.ReadAsStreamAsync(),
            stream = new FileStream(fullPath, FileMode.Create);
        await contentStream.CopyToAsync(stream);

        await File.WriteAllTextAsync(Path.Join(fileDirectory, Constants.LogFileName), string.Format(Constants.LogFormat, $"{DateTime.UtcNow:s}z", cachedFileName));
    }

    return Results.File(Path.Join(fileDirectory, cachedFileName), "image/jpg");
})
.WithName("GetRandomImage")
.WithOpenApi();

app.MapGet("/cache-state", async (IConfiguration configuration) =>
{
    var filePath =
        Path.Join(configuration.GetValue<string>(Constants.FilePathConfigurationKey), Constants.LogFileName);

    if (!File.Exists(filePath))
    {
        return "Image is not cached yet!";
    }

    var content = await File.ReadAllTextAsync(filePath);

    return content;
})
    .WithName("GetCacheState")
    .WithOpenApi();

app.Run();
