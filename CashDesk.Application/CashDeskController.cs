using Domain.CashDesk;

// Controls the cash desk and its peripherals

namespace CashDesk.Application;

public class CashDeskController : ICashDeskController
{
    private readonly ICashBoxController _cashBoxController;
    private readonly IBarcodeScannerController _barcodeScannerController;
    private readonly IPrinterController _printerController;
    private readonly IDisplayController _displayController;
    private readonly ICardReaderController _cardReaderController;
    private readonly ITransactionService _transactionService;
    
    public CashDeskController(ICashBoxController cashBoxController, IBarcodeScannerController barcodeScannerController,
        IPrinterController printerController, IDisplayController displayController, ICardReaderController cardReaderController, ITransactionService transactionService)
    {
        _cashBoxController = cashBoxController;
        _barcodeScannerController = barcodeScannerController;
        _printerController = printerController;
        _displayController = displayController;
        _cardReaderController = cardReaderController;
        _transactionService = transactionService;
        
        _cashBoxController.ActionTriggered += OnActionTriggered;
    }

    public void Start()
    {
        _cashBoxController.StartListeningToCashbox();
        Console.WriteLine("CashDesk state machine started.");
    }
    
    private void OnActionTriggered(object? sender, CashboxAction button)
    {
        Console.WriteLine($"Cashbox button pressed: {button}");
    }
}