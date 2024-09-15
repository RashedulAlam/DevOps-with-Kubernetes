using System.Security.Cryptography;
using System.Text;
using Log_output;
using Log_output.Services.V2;

var builder = WebApplication.CreateBuilder(args);
var isReader = bool.Parse(Environment.GetEnvironmentVariable(Constants.ReaderEnvironmentVariable));

if (isReader)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}

if (!isReader)
{
    builder.Services.AddSingleton<StatusLoggerBackgroundService>();
    builder.Services.AddHostedService(x => x.GetRequiredService<StatusLoggerBackgroundService>());
}

var app = builder.Build();

if (isReader)
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapGet("/", async (IConfiguration configuration) =>
        {
            var filePath = configuration.GetValue<string>(Constants.FilePathKey);
            var fileFullPath = Path.Join(filePath, Constants.FileName);
            var pingpongFileFullPath = Path.Join(filePath, Constants.PingPongFileName);
            var pingPongContents = await File.ReadAllTextAsync(pingpongFileFullPath);

            var bytes = await File.ReadAllBytesAsync(fileFullPath);

            using SHA256 sha256 = SHA256.Create();
            var hashes = sha256.ComputeHash(bytes);
            var stringBuilder = new StringBuilder();

            foreach (var b in hashes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            var text = stringBuilder.ToString();

            return $"{DateTime.UtcNow:s}Z: {text}.\n{pingPongContents}";
        })
        .WithName("GetHashLog")
        .WithOpenApi();
}

app.Run();
