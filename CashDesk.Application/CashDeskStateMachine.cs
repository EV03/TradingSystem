
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
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
   
   private readonly CashDeskExpressModeStateMachine _expressModeStateMachine;
   
   private readonly ICashBoxController _cashBoxController;
   private readonly IPrinterController _printerController;
   private readonly IDisplayController _displayController;
   private readonly IBarcodeScannerController _barcodeScannerController;
   private readonly ICardReaderController _cardReaderController;
   
   private readonly ISaleService _saleService;
   private readonly IPaymentService _paymentService;
   private readonly IExpressModeService _expressModeService; 
   private readonly ITransactionRepository _transactionRepository;
   
   // to store the sale items in the sale and create a transaction
   List<SaleItem>  _saleItem;
   private String _paymentMethod; 

   public CashDeskSalesStateMachine(ICashBoxController cashBoxController, IPrinterController printerController,
       IBarcodeScannerController barcodeScannerController, ICardReaderController cardReaderController, CashDeskExpressModeStateMachine expressModeStateMachine,
       IDisplayController displayController , ISaleService saleService, IPaymentService paymentService, IExpressModeService expressModeService, ITransactionRepository transactionRepository) 
   { 
       _cashBoxController = cashBoxController;
       _printerController = printerController;
       _barcodeScannerController = barcodeScannerController;
       _displayController = displayController;
       _cardReaderController = cardReaderController;
       
       // services 
       _saleService = saleService;
       _paymentService = paymentService;
       _expressModeService = expressModeService;
       _transactionRepository = transactionRepository;
       _paymentMethod = null!;
       _saleItem = null!;

       _stateMachine = new StateMachine<CashDeskSaleState, CashDeskAction>(CashDeskSaleState.Init);
         _expressModeStateMachine = expressModeStateMachine;
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
               if (_expressModeStateMachine.State() == CashDeskExpressModeState.Enabled)
               {
                   _displayController.DisplayText("Waiting for Sale to Start. Express Mode Enabled");
               }
               else
               {
                   _displayController.DisplayText("Waiting for Sale to start. Express Mode Disabled");
               }
               Console.WriteLine("Press 'Start New Sale' to begin a new sale. expressmode state: " + _expressModeStateMachine.State());
               _cashBoxController.StartListeningToCashbox();
           });


       _stateMachine.Configure(CashDeskSaleState.SaleActive)
           .PermitReentry(_productScannedTrigger.Trigger)
           .Permit(CashDeskAction.FinishSale, CashDeskSaleState.PreparePayment)

           .OnEntryFrom(CashDeskAction.StartNewSale, () =>
           {
                Console.WriteLine("Sale started, scan products");
                _displayController.DisplayText("Sale started, scan products");
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
               finally
               {
                   _barcodeScannerController.StartListeningToBarcodes();
               }
           });
          

       _stateMachine.Configure(CashDeskSaleState.PreparePayment)
           .Permit(CashDeskAction.PayWithCard, CashDeskSaleState.PaymentInProgress)
           .Permit(CashDeskAction.PayWithCash, CashDeskSaleState.PaymentInProgress)
           .OnEntryFrom( CashDeskAction.FinishSale, () =>
           {
               Console.WriteLine("Sale finished, payment method has to be chosen");
               _displayController.DisplayText("Choose payment method");
               _barcodeScannerController.StopListeningToBarcodes();
              
           })
           .OnEntryFrom(CashDeskAction.CancelPayment, () =>
           {
               _cashBoxController.StartListeningToCashbox();
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
               Console.WriteLine("card payment started, waiting for card swipe");
               _displayController.DisplayText("Card payment, please swipe card");
               try
               { 
                   
                   var  result = _paymentService.PayCardAsync(_saleService.GetSaleTotal());
                   
                   // block until payment is completed
                   result.Wait();
                   // simulate card payment or cancel time
                   Console.WriteLine("Uploading data, wait a few seconds...");
                   Task.Delay(3000).Wait();
                   
                   _cardReaderController.Confirm("");
                   _paymentMethod = "CardPayment";
                   _stateMachine.Fire(CashDeskAction.CompletePayment);
               }
               catch (Exception ex)
               {
                     Console.WriteLine("Card payment got canceled, choose card or cash payment again");  
                   _stateMachine.Fire(CashDeskAction.CancelPayment);
               }
           })
           .OnEntryFrom(CashDeskAction.PayWithCash, () =>
           {
                Console.WriteLine("cash payment started");
               _displayController.DisplayText("cash payment");
               _paymentService.PayCashAsync(_saleService.GetSaleTotal());
               _paymentMethod = "CashPayment";
                _stateMachine.Fire(CashDeskAction.CompletePayment);
           });
         
            _stateMachine.Configure(CashDeskSaleState.PrintingReceipt)
                .Permit(CashDeskAction.Complete, CashDeskSaleState.Idle)
                .OnEntry(async () =>
                { 
                    Console.WriteLine("payment succesful");
                    
                    await _saleService.FinishSaleAsync();
                    
                    await Task.Delay(1000);
                    Console.WriteLine("Printing receipt...\n");
                    _displayController.DisplayText("Printing receipt...");

                    // simulating printing time
                    await Task.Delay(3000);
                    _printerController.Print("Receipt");
                    Console.WriteLine("Receipt printed. Returning to Idle state...\n\n");

                    if (_saleService.Sale?.Items != null) _saleItem = _saleService.Sale.Items;
                    _transactionRepository.SaveTransaction( new Transaction(_saleItem, _paymentMethod));
                    _saleItem = null!;
                    _paymentMethod = "";

                    if(_expressModeStateMachine.State() == CashDeskExpressModeState.Disabled)
                    {
                       var expressMode =  _expressModeService.IsExpressMode();
                       if(expressMode) _expressModeStateMachine.Fire(CashDeskExpressModeActions.EnableExpressMode);
                    }
                    // for new sales
                    _stateMachine.Fire(CashDeskAction.Complete);
                });
   }

   public void Fire(CashDeskAction action)
   {
       if(_stateMachine.CanFire(action)) _stateMachine.Fire(action);
       else Console.WriteLine($"Action {action} cannot be fired in state {_stateMachine.State}");
   }
   
    public void Fire(CashDeskAction action, string barcode)
    {
        if(_stateMachine.CanFire(action)) _stateMachine.Fire(_productScannedTrigger, barcode);
        else Console.WriteLine($"Action {action} cannot be fired in state {_stateMachine.State}");
    }
   
   public bool CanFire(CashDeskAction action)
   {
       return _stateMachine.CanFire(action) && action != CashDeskAction.CancelPayment;
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
    private readonly StateMachine<CashDeskExpressModeState, CashDeskExpressModeActions> _stateMachine;
    
    private readonly IDisplayController _displayController;
    
    public CashDeskExpressModeStateMachine(IDisplayController displayController)
    {
        _displayController = displayController;
        _stateMachine = new StateMachine<CashDeskExpressModeState, CashDeskExpressModeActions>(CashDeskExpressModeState.Disabled);
        ConfigureStateMachine();
    }

    private void ConfigureStateMachine()
    {
        _stateMachine.Configure(CashDeskExpressModeState.Disabled)
            .Permit(CashDeskExpressModeActions.EnableExpressMode, CashDeskExpressModeState.Enabled)
            .OnEntryFrom(CashDeskExpressModeActions.DisableExpressMode, () =>
            {
                Console.WriteLine("Express Mode disabled");
                _displayController.DisplayText("Waiting for Sale to Start. Express Mode Disabled");
            });
        
        _stateMachine.Configure(CashDeskExpressModeState.Enabled)
            .Permit(CashDeskExpressModeActions.DisableExpressMode, CashDeskExpressModeState.Disabled)
            .OnEntry(() =>
            {
                Console.WriteLine("Express Mode Enabled");
            });
    }
    
    public void Fire(CashDeskExpressModeActions action)
    {
        if(_stateMachine.CanFire(action)) _stateMachine.Fire(action);
        else Console.WriteLine($"Action {action} cannot be fired in state {_stateMachine.State}");
    }
    
    public bool CanFire(CashDeskExpressModeActions action)
    {
        return _stateMachine.CanFire(action);
    }
    
    // returns state of the express mode
    public CashDeskExpressModeState State()
    {
        return _stateMachine.State;
    }
}