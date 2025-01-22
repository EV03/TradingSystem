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
         _salesStateMachine.Fire(action);
    }
    
    public void OnBarcodeScanned(string barcode)
    {
        _salesStateMachine.Fire(CashDeskAction.ProductScanned, barcode);
    }
    
    public CashDeskExpressModeActions MapExpressModeAction(CashDeskAction action)
    {
        return action switch
        {
            CashDeskAction.DisableExpressMode => CashDeskExpressModeActions.DisableExpressMode,
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
        };
    }
}