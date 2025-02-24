﻿using System.Net.NetworkInformation;
using Microsoft.Extensions.DependencyInjection;
using Tecan.Sila2.Client;
using Tecan.Sila2.Client.ExecutionManagement;
using Tecan.Sila2.Discovery;
using CashDesk.Application;
using CashDesk.Integration;
using CashDesk.Integration.Bank;
using CashDeskHardwareControllers;
using CashDeskHardwareControllers.BarcodeScannerService;
using CashDeskHardwareControllers.CardReaderService;
using CashDeskHardwareControllers.CashBoxService;
using CashDeskHardwareControllers.DisplayService;
using CashDeskHardwareControllers.PrinterService;
using CashDeskHardwareControllers.PrinterService.PrintingService;
using Domain.CashDesk;
using Microsoft.Extensions.Configuration;
using Shared.Contracts.Interfaces;
using Shared.Contracts.Protos;
using IDisplayController = Domain.CashDesk.IDisplayController;

namespace CashDesk.Infrastructure;

// Each cash desk runs as a separate process
public class CashDeskDependencyInjection
{
    public static ServiceProvider ConfigureServices(IConfiguration configuration, string profileName)
    {
        var services = new ServiceCollection();

        // SiLA Connector and Discovery
        services.AddSingleton<ServerConnector>(provider => new ServerConnector(new DiscoveryExecutionManager()));
        services.AddSingleton<ServerDiscovery>(provider => new ServerDiscovery(provider.GetRequiredService<ServerConnector>()));
        services.AddSingleton<ExecutionManagerFactory>(provider => new ExecutionManagerFactory(Enumerable.Empty<IClientRequestInterceptor>()));

        // Register Terminal and Bank Servers
        services.AddSingleton<ServerDataWrapper.TerminalServerData>( provider =>
        {
            var discovery = provider.GetRequiredService<ServerDiscovery>();
            var servers = discovery.GetServers(TimeSpan.FromSeconds(10), n => n.NetworkInterfaceType == NetworkInterfaceType.Loopback);
            var terminalServer = servers.First(s => s.Info.Type == "Terminal");
            return new ServerDataWrapper.TerminalServerData(terminalServer);
        });
       
        services.AddSingleton<ServerDataWrapper.BankServerData>( provider =>
        {
            var discovery = provider.GetRequiredService<ServerDiscovery>();
            var servers = discovery.GetServers(TimeSpan.FromSeconds(10), n => n.NetworkInterfaceType == NetworkInterfaceType.Loopback);
            var bankServer = servers.FirstOrDefault(s => s.Info.Type == "BankServer");
            if (bankServer == null)
            {
                throw new InvalidOperationException("No BankServer found during discovery.");
            }

            return new ServerDataWrapper.BankServerData(bankServer);
        });
        
        // SiLA Clients
        services.AddSingleton<ICashboxService>(provider =>
        {
            var terminalServer = provider.GetRequiredService<ServerDataWrapper.TerminalServerData>();
            var executionManagerFactory = provider.GetRequiredService<ExecutionManagerFactory>();
            var executionManager = executionManagerFactory.CreateExecutionManager(terminalServer.Data);
            return new CashboxServiceClient(terminalServer.Data.Channel, executionManager);
        });

        services.AddSingleton<IBankServer>(provider =>
        {
            var bankServerData = provider.GetRequiredService<ServerDataWrapper.BankServerData>();
            var executionManagerFactory = provider.GetRequiredService<ExecutionManagerFactory>();
            var executionManager = executionManagerFactory.CreateExecutionManager(bankServerData.Data);
            return new BankServerClient(bankServerData.Data.Channel, executionManager);
        });

        services.AddSingleton<IBarcodeScannerService>(provider =>
        {
            var terminalServer = provider.GetRequiredService<ServerDataWrapper.TerminalServerData>();
            var executionManagerFactory = provider.GetRequiredService<ExecutionManagerFactory>();
            var executionManager = executionManagerFactory.CreateExecutionManager(terminalServer.Data);
            return new BarcodeScannerServiceClient(terminalServer.Data.Channel, executionManager);
        });

        services.AddSingleton<IDisplayService>(provider =>
        {
            var terminalServer = provider.GetRequiredService<ServerDataWrapper.TerminalServerData>();
            var executionManagerFactory = provider.GetRequiredService<ExecutionManagerFactory>();
            var executionManager = executionManagerFactory.CreateExecutionManager(terminalServer.Data);
            return new DisplayControllerClient(terminalServer.Data.Channel, executionManager);
        });

        services.AddSingleton<IPrintingService>(provider =>
        {
            var terminalServer = provider.GetRequiredService<ServerDataWrapper.TerminalServerData>();
            var executionManagerFactory = provider.GetRequiredService<ExecutionManagerFactory>();
            var executionManager = executionManagerFactory.CreateExecutionManager(terminalServer.Data);
            return new PrintingServiceClient(terminalServer.Data.Channel, executionManager);
        });

        services.AddSingleton<ICardReaderService>(provider =>
        {
            var terminalServer = provider.GetRequiredService<ServerDataWrapper.TerminalServerData>();
            var executionManagerFactory = provider.GetRequiredService<ExecutionManagerFactory>();
            var executionManager = executionManagerFactory.CreateExecutionManager(terminalServer.Data);
            return new CardReaderServiceClient(terminalServer.Data.Channel, executionManager);
        });

        // Register hardware controllers
        services.AddSingleton<ICashBoxController, SilaCashBoxAdapter>();
        services.AddSingleton<IBarcodeScannerController, SilaBarcodeScannerAdapter>();
        services.AddSingleton<IPrinterController, SilaPrinterAdapter>();
        services.AddSingleton<IDisplayController, SilaDisplayAdapter>();
        services.AddSingleton<ICardReaderController, SilaCardReaderAdapter>();

        // Register application services
        services.AddSingleton<ISaleService, SaleService>();
        services.AddSingleton<Sale>();
        services.AddSingleton<IBankService, SilaBankServiceAdapter>();
        services.AddSingleton<IPaymentService, PaymentService>();
        services.AddSingleton<IExpressModeService, ExpressModeService>();

        // Register integration services
        services.AddSingleton<ITransactionRepository, TransactionCacheRepository>();
        services.AddSingleton<IStoreCommunication, GrpcStoreClient>();

        // Register state machines
        services.AddSingleton<CashDeskSalesStateMachine>();
        services.AddSingleton<CashDeskSalesStateMachine.CashDeskExpressModeStateMachine>();

        // Register the CashDeskController
        services.AddSingleton<CashDeskController>();
        
        // Register gRPC client
        var storeAddress = configuration.GetSection($"Profiles:{profileName}:StoreAddress").Value;

        if (string.IsNullOrEmpty(storeAddress))
        {
            throw new InvalidOperationException($"Store address not found in configuration for profile: {profileName}");
        }

        if (!Uri.TryCreate(storeAddress, UriKind.Absolute, out var uri))
        {
            throw new InvalidOperationException($"Invalid store address in configuration: {storeAddress}");
        }

        services.AddGrpcClient<Product.ProductClient>(o =>
        {
            o.Address = uri;
        });

        return services.BuildServiceProvider();
    }
}