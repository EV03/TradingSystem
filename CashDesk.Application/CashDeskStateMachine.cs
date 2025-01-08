using System.Runtime.Serialization;
using System.Security.Permissions;
using Domain.CashDesk;

namespace CashDesk.Application;
using Stateless;

public enum CashDeskSaleState
{
    Idle,
    SaleActive,
    PreparePayment,
    PaymentInProgress,
    PrintingReceipt
}
public class CashDeskSalesStateMachine
{
   private readonly StateMachine<CashDeskSaleState, CashDeskAction> _stateMachine;
   private readonly ICashBoxController _cashBoxController;
   private readonly IPrinterController _printerController;
   private readonly StateMachine<CashDeskSaleState, CashDeskAction>.TriggerWithParameters<string> _productScannedTrigger;
   private readonly ISaleService _saleService;

   public CashDeskSalesStateMachine() 
   {
     _stateMachine = new StateMachine<CashDeskSaleState, CashDeskAction>(CashDeskSaleState.Idle);
     _productScannedTrigger = _stateMachine.SetTriggerParameters<string>(CashDeskAction.ProductScanned);
     ConfigureStateMachine();
   }

   private void ConfigureStateMachine()
   {
       _stateMachine.Configure(CashDeskSaleState.Idle)
           .Permit(CashDeskAction.StartNewSale, CashDeskSaleState.SaleActive)
           .OnEntry(()=> _cashBoxController.StartListeningToCashbox());


       _stateMachine.Configure(CashDeskSaleState.SaleActive)
           .PermitIf(_productScannedTrigger, CashDeskSaleState.SaleActive,
               barcode => _saleService.isValidBarcode(barcode), "Invalid barcode")
           .Permit(CashDeskAction.FinishSale, CashDeskSaleState.PreparePayment)
           
           .OnEntryFrom(CashDeskAction.StartNewSale, () => _saleService.StartSale())
           .OnEntryFrom(_productScannedTrigger, barcode =>
           {
               var result = _saleService.AddProductToSale(barcode).Result;
               if (result.IsSuccess)
               {
                   Console.WriteLine($"Product with barcode {barcode} added to sale.");
               }
               else if (result.IsCanceled)
               {
                   Console.WriteLine($"Product with barcode {barcode} not found in store.");
               }
               else
               {
                   // rpc request failed
                   Console.WriteLine(
                       $"Failed to add product with barcode {barcode} to sale. Reason: {result.ErrorMessage}");
                   // todo: retry 
               }
           });

       _stateMachine.Configure(CashDeskSaleState.PreparePayment)
           .Permit(CashDeskAction.PayWithCard, CashDeskSaleState.PaymentInProgress)
           .Permit(CashDeskAction.PayWithCash, CashDeskSaleState.PaymentInProgress);
         
         _stateMachine.Configure(CashDeskSaleState.PaymentInProgress)
             .Permit(CashDeskAction.CompletePayment, CashDeskSaleState.PrintingReceipt)
             .Permit(CashDeskAction.CancelPayment, CashDeskSaleState.PaymentInProgress);
         
            _stateMachine.Configure(CashDeskSaleState.PrintingReceipt)
                .Permit(CashDeskAction.StartNewSale, CashDeskSaleState.SaleActive)
                .OnEntry(async () =>
                {
                    Console.WriteLine("Printing receipt...");
                    _printerController.Print("Receipt");

                    // Simulate printing time
                    await Task.Delay(5000); 
                    Console.WriteLine("Receipt printed. Returning to Idle state...");
                    _stateMachine.Fire(CashDeskAction.GoToIdle);
                });
            
   }

   public void Fire(CashDeskAction action)
   {
       _stateMachine.Fire(action);
   }
   
   public bool CanFire(CashDeskAction action)
   {
       return _stateMachine.CanFire(action);
   }
}

/// <summary>
/// ////////////////////////////////////////////////////////////////
/// </summary>
public enum CashDeskExpressModeState
{
    Disabled,
    Enabled
}

public enum CashDeskExpressModeActions
{
    EnableExpressMode,
    DisableExpressMode
}

public class CashDeskExpressModeStateMachine
{
    private readonly StateMachine<CashDeskExpressModeState, CashDeskAction> _stateMachine;
    
    public CashDeskExpressModeStateMachine()
    {
        _stateMachine = new StateMachine<CashDeskExpressModeState, CashDeskAction>(CashDeskExpressModeState.Disabled);
        ConfigureStateMachine();
    }

    private void ConfigureStateMachine()
    {
        _stateMachine.Configure(CashDeskExpressModeState.Disabled)
            .Permit(CashDeskAction.EnableExpressMode, CashDeskExpressModeState.Enabled);
        
        _stateMachine.Configure(CashDeskExpressModeState.Enabled)
            .Permit(CashDeskAction.DisableExpressMode, CashDeskExpressModeState.Disabled);
    }
    
    public void Fire(CashDeskAction action)
    {
        _stateMachine.Fire(action);
    }
    
    public bool CanFire(CashDeskAction action)
    {
        return _stateMachine.CanFire(action);
    }
}