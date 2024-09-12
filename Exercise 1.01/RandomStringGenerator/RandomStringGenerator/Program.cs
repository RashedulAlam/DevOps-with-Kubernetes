while (true)
{
    var guid = Guid.NewGuid().ToString("D");

    Console.WriteLine($"{DateTime.UtcNow:s}Z : {guid}");
    
    await Task.Delay(5000);
}
