using System.Reflection;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoService;
using TodoService.Business;
using TodoService.Business.Model;
using TodoService.Business.Services;
using TodoService.Domain.Entity;
using TodoService.Infrastructure;
using TodoService.Infrastructure.Context;
using TodoService.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.RegisterBusinessServices();
builder.Services.RegisterInfrastructureServices();
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.MediaTypeOptions.AddText("application/json");
    logging.RequestHeaders.Add("User-Agent");
});


builder.Services.AddDbContext<TodoContext>(x =>
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
        optionsBuilder => optionsBuilder.MigrationsAssembly(Assembly.GetAssembly(typeof(TodoContext))?.ToString()));
});

var app = builder.Build();
app.UseHttpLogging();

app.UsePathBase("/todolist-service");
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(x =>
{
    x.AllowAnyHeader();
    x.AllowAnyMethod();
    x.AllowAnyOrigin();
});

app.MapGet("/todos", async ([FromServices] ITodoStorage todoStorage) => await todoStorage.GetAll())
    .WithName("GetTodos")
    .WithOpenApi();

await using var scope = app.Services.CreateAsyncScope();
await scope.ServiceProvider.GetRequiredService<TodoContext>().Database.MigrateAsync();

if (Environment.GetEnvironmentVariables().Contains("cronjob"))
{
    var httpClient = scope.ServiceProvider.GetRequiredService<HttpClient>();
    var response = await httpClient.GetAsync("https://en.wikipedia.org/wiki/Special:Random");
    var header = response.RequestMessage?.RequestUri?.AbsoluteUri;

    if (!string.IsNullOrWhiteSpace(header))
    {
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        await unitOfWork.GetRepository<Todo>().Add(new Todo() { Title = $"Read {header}" });
    }

    Environment.Exit(default);
}

app.MapPost("/todos", async ([FromBody] TodoCreateRequest todo, [FromServices] ITodoStorage todoStorage) =>
    {
        var newTodo = await todoStorage.Create(todo); 

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
