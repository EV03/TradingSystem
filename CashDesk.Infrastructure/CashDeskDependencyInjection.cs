using Microsoft.Extensions.DependencyInjection;
using CashDesk.Application;
using CashDesk.Infrastructure.Bank;
using CashDesk.Integration;
using CashDeskHardwareControllers;
using CashDeskHardwareControllers.BarcodeScannerService;
using CashDeskHardwareControllers.CardReaderService;
using CashDeskHardwareControllers.CashBoxService;
using CashDeskHardwareControllers.DisplayService;
using CashDeskHardwareControllers.PrinterService;
using CashDeskHardwareControllers.PrinterService.PrintingService;
using Domain.CashDesk;
using Shared.Contracts.Interfaces;
using IDisplayController = Domain.CashDesk.IDisplayController;

namespace CashDesk.Infrastructure;

//  each cash desk runs as a seperate process 
public class CashDeskDependencyInjection
{
    public static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Register hardware controllers
        services.AddSingleton<ICashBoxController, SilaCashBoxAdapter>();
        services.AddSingleton<IBarcodeScannerController, SilaBarcodeScannerAdapter>();
        services.AddSingleton<IPrinterController, SilaPrinterAdapter>();
        services.AddSingleton<IDisplayController, SilaDisplayAdapter>();
        services.AddSingleton<ICardReaderController, SilaCardReaderAdapter>();
        
        // register sila services
        services.AddSingleton<ICashboxService, CashboxServiceClient>();
        services.AddSingleton<IBarcodeScannerService, BarcodeScannerServiceClient>();
        services.AddSingleton<IPrintingService, PrintingServiceClient>();
        services.AddSingleton<IDisplayService, DisplayControllerClient>();
        services.AddSingleton<ICardReaderService, CardReaderServiceClient>();
        services.AddSingleton<IBankServer, BankServerClient>();
        
        // Register application services
        services.AddSingleton<ISaleService, SaleService>();
        services.AddSingleton<IBankService, SilaBankServiceAdapter>();
        services.AddSingleton<IPaymentService, PaymentService>();
        
        // register integration services
        services.AddSingleton<ITransactionRepository, TransactionCacheRepository>();
        services.AddSingleton<IStoreCommunication, GrpcStoreClient>();

        // Register state machines
        services.AddSingleton<CashDeskSalesStateMachine>();
        services.AddSingleton<CashDeskExpressModeStateMachine>();

        // Register the CashDeskController
        services.AddSingleton<CashDeskController>();

        return services.BuildServiceProvider();
    }
}