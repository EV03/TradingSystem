using Domain.CashDesk;
using Tecan.Sila2;

namespace CashDeskHardwareControllers.CashBoxService;

public class SilaCashBoxAdapter : ICashBoxController
{
    private readonly ICashboxService _cashboxService;
    private readonly ITransactionService _transactionService;
    private IIntermediateObservableCommand<CashboxButton>? _buttonStream;
    
    public SilaCashBoxAdapter(ICashboxService cashboxService, ITransactionService transactionService)
    {
        _cashboxService = cashboxService;
        _transactionService = transactionService;
    }

    public event EventHandler<CashDeskAction>? ActionTriggered;

    public void startCashbox()
    {
        if (_buttonStream != null)
        {
            throw new InvalidOperationException("Already listening to cashbox buttons.");
        }

        _buttonStream = _cashboxService.ListenToCashdeskButtons();
        Console.WriteLine("Started listening to cash box");
        
        Task.Run(async () =>
        {
            try
            {
                while (await _buttonStream.IntermediateValues.WaitToReadAsync())
                {
                    if (_buttonStream.IntermediateValues.TryRead(out var button))
                    {
                        Console.WriteLine($"Cashbox button pressed: {button}");
                        ActionTriggered?.Invoke(this, MapSilaButtonToCashDeskAction(button)); 
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Cashbox button listening was canceled.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while listening to cashbox buttons: {ex.Message}");
            }
        });
    }
    
    public void StopCashbox()
    {
        if (_buttonStream == null)
        {
            throw new InvalidOperationException("Not listening to cashbox buttons.");
        }

        _buttonStream.Cancel();
        _buttonStream = null;
        Console.WriteLine("Stopped listening to cash box");
    }
    
    private CashDeskAction MapSilaButtonToCashDeskAction(CashboxButton silaButton)
    {
        return silaButton switch
        {
            CashboxButton.StartNewSale => CashDeskAction.StartNewSale,
            CashboxButton.FinishSale => CashDeskAction.FinishSale,
            CashboxButton.PayWithCash => CashDeskAction.PayWithCash,
            CashboxButton.PayWithCard => CashDeskAction.PayWithCard,
            CashboxButton.DisableExpressMode => CashDeskAction.DisableExpressMode,
            _ => throw new ArgumentOutOfRangeException(nameof(silaButton), silaButton, null)
        };
    }
}