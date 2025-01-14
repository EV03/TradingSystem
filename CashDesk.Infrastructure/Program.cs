
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
    }
}