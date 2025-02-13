
using CashDesk.Application;
using CashDesk.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {
        var profileName = args.Length > 0 ? args[0] : "CashDesk1";

    var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var serviceProvider = CashDeskDependencyInjection.ConfigureServices(configuration,profileName);
        Console.WriteLine($"CashDesk Server started with profile {profileName}");
        var cashDeskController = serviceProvider.GetRequiredService<CashDeskController>();
        
       // wait for Ctrl+C
        using var cts = new CancellationTokenSource();

        // Listen for Ctrl+C
        Console.CancelKeyPress += (sender, e) =>
        {
            // Prevent the process from terminating immediately
            e.Cancel = true;
            cts.Cancel();
        };
        // Block until the token is canceled by Ctrl+C
        cts.Token.WaitHandle.WaitOne();
        Console.WriteLine("Stopping server.");
    }
}