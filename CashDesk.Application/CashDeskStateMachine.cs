
using Domain.CashDesk;

namespace CashDesk.Application;
using Stateless;

public enum CashDeskSaleState
{
    Init,
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
   private readonly IPaymentService _paymentService;

   public CashDeskSalesStateMachine(ICashBoxController cashBoxController, IPrinterController printerController,
       IBarcodeScannerController barcodeScannerController, ICardReaderController cardReaderController, 
       IDisplayController displayController , ISaleService saleService, IPaymentService paymentService) 
   { 
       _cashBoxController = cashBoxController;
       _printerController = printerController;
       _barcodeScannerController = barcodeScannerController;
       _displayController = displayController;
       _cardReaderController = cardReaderController;
       
       // services 
       _saleService = saleService;
       _paymentService = paymentService;
       
       _stateMachine = new StateMachine<CashDeskSaleState, CashDeskAction>(CashDeskSaleState.Init); 
       _productScannedTrigger = _stateMachine.SetTriggerParameters<string>(CashDeskAction.ProductScanned);
       ConfigureStateMachine();
       Console.WriteLine("CashDesk initialized.");
       _stateMachine.Activate();
       _stateMachine.Fire(CashDeskAction.StartNewSale); // go to idle state to call onEntry
   }
   
   private void ConfigureStateMachine()
   {
       _stateMachine.Configure(CashDeskSaleState.Init)
           .Permit(CashDeskAction.StartNewSale, CashDeskSaleState.Idle);
       
       _stateMachine.Configure(CashDeskSaleState.Idle)
           .Permit(CashDeskAction.StartNewSale, CashDeskSaleState.SaleActive)
           .OnEntry(() =>
           {
               Console.WriteLine("Press buttons");
               _cashBoxController.StartListeningToCashbox();
           });


       _stateMachine.Configure(CashDeskSaleState.SaleActive)
           .PermitReentry(_productScannedTrigger.Trigger)
           .Permit(CashDeskAction.FinishSale, CashDeskSaleState.PreparePayment)

           .OnEntryFrom(CashDeskAction.StartNewSale, () =>
           {
               _barcodeScannerController.StartListeningToBarcodes();
               _saleService.StartSale();
           })
           .OnEntryFrom(_productScannedTrigger, barcode =>
           {
               // Stop listening to barcodes while processing the scanned product
               _displayController.DisplayText(barcode);
               _barcodeScannerController.StopListeningToBarcodes();

               try
               {
                   var result = _saleService.AddProductToSale(barcode);
                   Console.WriteLine($"Product with barcode {barcode} added to sale.");
               }
               catch (Exception ex)
               {
                   Console.WriteLine($"Failed to add product with barcode {barcode} to sale, Reason: {ex.Message}");
               }

               // Resume listening to barcodes
               _barcodeScannerController.StartListeningToBarcodes();
           });
          

       _stateMachine.Configure(CashDeskSaleState.PreparePayment)
           .Permit(CashDeskAction.PayWithCard, CashDeskSaleState.PaymentInProgress)
           .Permit(CashDeskAction.PayWithCash, CashDeskSaleState.PaymentInProgress)
           .OnEntry(() =>
           {
               _barcodeScannerController.StopListeningToBarcodes();
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
               _displayController.DisplayText("Card payment, please swipe card");
               try
               { 
                   var  result = _paymentService.PayCardAsync(_saleService.GetSaleTotal());
                   
                   // block until payment is completed
                   result.Wait();
                   
                   if (result.IsCanceled)
                   {
                        Console.WriteLine("Card payment was canceled by the client.");
                       _stateMachine.Fire(CashDeskAction.CancelPayment);
                   }
                   _stateMachine.Fire(CashDeskAction.CompletePayment);
               }
               catch (Exception ex)
               {
                   Console.WriteLine("Failed to pay with card. Reason: " + ex.Message);
                   _stateMachine.Fire(CashDeskAction.CancelPayment);
               }
           })
           .OnEntryFrom(CashDeskAction.PayWithCash, () =>
           {
               _displayController.DisplayText("cash payment");
               _paymentService.PayCashAsync(_saleService.GetSaleTotal());
                _stateMachine.Fire(CashDeskAction.CompletePayment);
           });
         
            _stateMachine.Configure(CashDeskSaleState.PrintingReceipt)
                .Permit(CashDeskAction.Complete, CashDeskSaleState.Idle)
                .OnEntry(async () =>
                { 
                    // if exception occurs, the state machine will just continue normally. the sale will be cached
                    var result = _saleService.FinishSaleAsync();
               
                    Console.WriteLine("Printing receipt...");
                    _displayController.DisplayText("Printing receipt...");
                    _printerController.Print("Receipt");

                    // Simulate printing time
                    await Task.Delay(5000); 
                    Console.WriteLine("Receipt printed. Returning to Idle state...");
                    
                    // for new sales
                    _stateMachine.Fire(CashDeskAction.Complete);
                });
   }

   public void Fire(CashDeskAction action)
   {
       if(!_stateMachine.CanFire(action))
       {
           Console.WriteLine($"Action {action} cannot be fired in state {_stateMachine.State}");
           throw new InvalidOperationException($"Action {action} cannot be fired in state {_stateMachine.State}");
       }
       _stateMachine.Fire(action);
   }
   
    public void Fire(CashDeskAction action, string barcode)
    {
         if(!_stateMachine.CanFire(action))
         {
              Console.WriteLine($"Action {action} cannot be fired in state {_stateMachine.State}");
              throw new InvalidOperationException($"Action {action} cannot be fired in state {_stateMachine.State}");
         }
         _stateMachine.Fire(_productScannedTrigger, barcode);
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
/// extra state machine for the express mode because stateless library cant handle multiple active states in one state machine
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