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

builder.Services.AddHttpClient();

var app = builder.Build();

if (isReader)
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapGet("/", async (IConfiguration configuration, HttpClient httpClient) =>
        {
            var filePath = configuration.GetValue<string>(Constants.FilePathKey);
            var fileFullPath = Path.Join(filePath, Constants.FileName);

            var requestUrl = $"{configuration.GetValue<string>(Constants.PingPongServiceKey)}/ping";

            var response = await httpClient.GetAsync(requestUrl);

            var responseContent = await response.Content.ReadAsStringAsync();

            var bytes = await File.ReadAllBytesAsync(fileFullPath);

            using SHA256 sha256 = SHA256.Create();
            var hashes = sha256.ComputeHash(bytes);
            var stringBuilder = new StringBuilder();

            foreach (var b in hashes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            var text = stringBuilder.ToString();

            var configMapFileContent = await File.ReadAllTextAsync(Path.Join(Path.Join(
                configuration.GetValue<string>(Constants.ConfigurationFilePathKey),
                Constants.ConfigurationFileName)));

            var environmentVariableValue = Environment.GetEnvironmentVariable(Constants.EnvironmentVariableName);

            return
                $"file content: {configMapFileContent}\nenv variable: {Constants.EnvironmentVariableName}={environmentVariableValue}\n{DateTime.UtcNow:s}Z: {text}.\n{responseContent}";
        })
        .WithName("GetHashLog")
        .WithOpenApi();
}

app.Run();
