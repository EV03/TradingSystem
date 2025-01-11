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
   private readonly StateMachine<CashDeskSaleState, CashDeskAction>.TriggerWithParameters<string> _productScannedTrigger;
   
   private readonly ICashBoxController _cashBoxController;
   private readonly IPrinterController _printerController;
   private readonly IDisplayController _displayController;
   private readonly IBarcodeScannerController _barcodeScannerController;
   private readonly ICardReaderController _cardReaderController;
   
   private readonly ISaleService _saleService;
   private readonly IBankService _bankService;
   private readonly IPaymentService _paymentService;

   public CashDeskSalesStateMachine(ICashBoxController cashBoxController, IPrinterController printerController,
       IBarcodeScannerController barcodeScannerController, ICardReaderController cardReaderController, 
       IDisplayController displayController , ISaleService saleService, IBankService bankService, IPaymentService paymentService) 
   { 
       _cashBoxController = cashBoxController;
       _printerController = printerController;
       _barcodeScannerController = barcodeScannerController;
       _displayController = displayController;
       _cardReaderController = cardReaderController;
       
       // services 
       _saleService = saleService;
       _bankService = bankService;
       _paymentService = paymentService;
       
       _stateMachine = new StateMachine<CashDeskSaleState, CashDeskAction>(CashDeskSaleState.Idle); 
       _productScannedTrigger = _stateMachine.SetTriggerParameters<string>(CashDeskAction.ProductScanned);
       ConfigureStateMachine();
   }
   
   private void ConfigureStateMachine()
   {
       _stateMachine.Configure(CashDeskSaleState.Idle)
           .Permit(CashDeskAction.StartNewSale, CashDeskSaleState.SaleActive)
           .OnEntry(()=> { _cashBoxController.StartListeningToCashbox(); });


       _stateMachine.Configure(CashDeskSaleState.SaleActive)
           .PermitIf(_productScannedTrigger, CashDeskSaleState.SaleActive,
               barcode => _saleService.isValidBarcode(barcode), "Invalid barcode")
           .Permit(CashDeskAction.FinishSale, CashDeskSaleState.PreparePayment)
           
           .OnEntryFrom(CashDeskAction.StartNewSale, () =>
           {
               _barcodeScannerController.StartListeningToBarcodes();
               _saleService.StartSale();
           })
           .OnEntryFrom(_productScannedTrigger, barcode =>
           {
               // Stop listening to barcodes while processing the scanned product
               _barcodeScannerController.StopListeningToBarcodes();
               
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
               // Resume listening to barcodes
               _barcodeScannerController.StartListeningToBarcodes();
           })
           .OnExit(() => _barcodeScannerController.StopListeningToBarcodes());

       _stateMachine.Configure(CashDeskSaleState.PreparePayment)
           .Permit(CashDeskAction.PayWithCard, CashDeskSaleState.PaymentInProgress)
           .Permit(CashDeskAction.PayWithCash, CashDeskSaleState.PaymentInProgress)
           .OnEntry(() =>
           {
               _displayController.DisplayText("Choose payment method");
           })
           .OnExit(() =>
           {
               // sale cant be finished or canceled until payment is completed
               _cashBoxController.StopListeningToCashbox();
           });

       _stateMachine.Configure(CashDeskSaleState.PaymentInProgress)
           .Permit(CashDeskAction.CompletePayment, CashDeskSaleState.PrintingReceipt)
           .Permit(CashDeskAction.CancelPayment, CashDeskSaleState.PreparePayment)
           .OnEntryFrom(CashDeskAction.PayWithCard, () =>
           {
               _displayController.DisplayText("Swipe card");
               var  result = _paymentService.PayCard(_saleService.GetSaleTotal()).Result;
               
               if (result)
               {
                   _stateMachine.Fire(CashDeskAction.CompletePayment);
               }
               else
               {
                   _stateMachine.Fire(CashDeskAction.CancelPayment);
               }
           })
           .OnEntryFrom(CashDeskAction.PayWithCash, () =>
           {
               _displayController.DisplayText("cash payment");
               _paymentService.PayCash(_saleService.GetSaleTotal());
           });
         
            _stateMachine.Configure(CashDeskSaleState.PrintingReceipt)
                .OnEntry(async () =>
                {
                    Console.WriteLine("Printing receipt...");
                    _displayController.DisplayText("Printing receipt...");
                    _printerController.Print("Receipt");

                    // Simulate printing time
                    await Task.Delay(5000); 
                    Console.WriteLine("Receipt printed. Returning to Idle state...");
                    _stateMachine.Fire(CashDeskAction.GoToIdle);
                });
            
   }

   public void Fire(CashDeskAction action)
   {
       if(!_stateMachine.CanFire(action))
       {
           Console.WriteLine($"Action {action} cannot be fired in state {_stateMachine.State}");
           return;
       }
       _stateMachine.Fire(action);
   }
   
   public bool CanFire(CashDeskAction action)
   {
       return _stateMachine.CanFire(action);
   }
   
   public void Info()
   {
       Console.WriteLine(
           "Press 'Start New Sale' to begin a new sale.\n" +
           "Click on the items to add them to the sale.\n" +
           "Press 'Finish Sale' when all items are added.\n" +
           "Press 'Pay With Cash' or 'Pay With Card' to choose a payment method.\n" +
           "If card payment is selected, swipe the card by pressing the corresponding button.\n" +
           "To switch from card payment to cash payment, press the cancel button.\n" +
           "If the payment is successful, the receipt will be printed.\n"
       );
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