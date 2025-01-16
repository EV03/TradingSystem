
using CashDesk.Application;
using CashDesk.Infrastructure;
using Domain.CashDesk;
using Microsoft.Extensions.DependencyInjection;

class Program
{
     static void Main(string[] args)
    {
        var serviceProvider = CashDeskDependencyInjection.ConfigureServices();
        Console.WriteLine("CashDesk Server started");
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

        Console.WriteLine("Press Ctrl+C to stop the server...");

        // Block until the token is canceled by Ctrl+C
        cts.Token.WaitHandle.WaitOne();
        Console.WriteLine("Stopping server.");
    }
}