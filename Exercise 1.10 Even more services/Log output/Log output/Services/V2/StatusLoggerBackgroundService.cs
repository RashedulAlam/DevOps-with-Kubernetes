using System.IO;

namespace Log_output.Services.V2
{
    public class StatusLoggerBackgroundService(IConfiguration configuration) : BackgroundService
    {
        private readonly string _filePath = configuration.GetValue<string>(Constants.FilePathKey) ?? string.Empty;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                string fullPath = Path.Combine(_filePath, Constants.FileName);

                if (!Directory.Exists(_filePath))
                {
                    Directory.CreateDirectory(_filePath);
                }

                if (!File.Exists(fullPath))
                {
                    File.Create(fullPath);
                }

                await using (StreamWriter sw = File.AppendText(fullPath))
                {
                    await sw.WriteLineAsync($"{DateTime.UtcNow:s}Z");
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
