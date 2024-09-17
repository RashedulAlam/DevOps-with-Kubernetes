namespace Log_output.Services
{
    public class StatusLoggerBackgroundService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var guid = Guid.NewGuid().ToString("D");

                Console.WriteLine($"{DateTime.UtcNow:s}Z : {guid}");

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
