using Domain.CashDesk;

// Controls the cash desk and its peripherals

namespace CashDesk.Application;

public class CashDeskController 
{
    private readonly ICashBoxController _cashBoxController;
    private readonly IBarcodeScannerController _barcodeScannerController;
   
    private readonly CashDeskSalesStateMachine _salesStateMachine;
    private readonly CashDeskExpressModeStateMachine _expressModeStateMachine;
    
    public CashDeskController(CashDeskSalesStateMachine salesStateMachine, CashDeskExpressModeStateMachine expressModeStateMachine,
        ICashBoxController cashBoxController, IBarcodeScannerController barcodeScannerController, ICardReaderController cardReaderController)
    {
        _cashBoxController = cashBoxController;
        _barcodeScannerController = barcodeScannerController;
        _salesStateMachine = salesStateMachine;
        _expressModeStateMachine = expressModeStateMachine;
        
        _cashBoxController.ActionTriggered += (sender, action) => OnActionTriggered(action);
        _cashBoxController.ListeningFailed += (sender, args) => Console.WriteLine(args);
        _barcodeScannerController.BarcodeScanned += (sender, barcode) => OnBarcodeScanned(barcode);
        _barcodeScannerController.BarcodeScanningFailed += (sender, args) => Console.WriteLine(args);
    }

    public void OnActionTriggered(CashDeskAction action)
    {
        Console.WriteLine($"Action triggered: {action}");
        

        if (_salesStateMachine.CanFire(action))
        {
            _salesStateMachine.Fire(action);
            return;
        }
        
        if(_expressModeStateMachine.CanFire(action))
        {
            _expressModeStateMachine.Fire(action);
            return;
        }
        
        Console.WriteLine($"Action {action} could not be handled by any state machine.");
    }
    
    public void OnBarcodeScanned(string barcode)
    {
        Console.WriteLine($"Barcode scanned: {barcode}");
        _salesStateMachine.Fire(CashDeskAction.ProductScanned, barcode);
    }
}