using Domain.CashDesk;

// Controls the cash desk and its peripherals

namespace CashDesk.Application;

public class CashDeskController : ICashDeskController
{
    private readonly ICashBoxController _cashBoxController;
    private readonly IBarcodeScannerController _barcodeScannerController;
    private readonly ICardReaderController _cardReaderController;
   
    private readonly CashDeskSalesStateMachine _salesStateMachine;
    private readonly CashDeskExpressModeStateMachine _expressModeStateMachine;
    
    
    
    public CashDeskController(CashDeskSalesStateMachine salesStateMachine, CashDeskExpressModeStateMachine expressModeStateMachine,
        ICashBoxController cashBoxController, IBarcodeScannerController barcodeScannerController, ICardReaderController cardReaderController)
    {
        _cashBoxController = cashBoxController;
        _barcodeScannerController = barcodeScannerController;
        _cardReaderController = cardReaderController;
        _salesStateMachine = salesStateMachine;
        _expressModeStateMachine = expressModeStateMachine;
    }
 

     public void Start()
    {
        Console.WriteLine("Init CashDes");
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
}