
using CashDesk.Application;
using CashDesk.Infrastructure;
using Domain.CashDesk;
using Microsoft.Extensions.DependencyInjection;

class Program
{
     static void Main(string[] args)
    {
        Console.WriteLine("CashDesk Server started");

        var serviceProvider = CashDeskDependencyInjection.ConfigureServices();
        var cashDeskController = serviceProvider.GetRequiredService<CashDeskController>();
        cashDeskController.Start();
    }
}