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

    public event EventHandler<CashboxAction>? ActionTriggered;

    public void StartListeningToCashbox()
    {
        if (_buttonStream != null)
        {
            throw new InvalidOperationException("Already listening to cashbox buttons.");
        }

        _buttonStream = _cashboxService.ListenToCashdeskButtons();
        
        Task.Run(async () =>
        {
            try
            {
                while (await _buttonStream.IntermediateValues.WaitToReadAsync())
                {
                    if (_buttonStream.IntermediateValues.TryRead(out var button))
                    {
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
    
    private CashboxAction MapSilaButtonToCashDeskAction(CashboxButton silaButton)
    {
        return silaButton switch
        {
            CashboxButton.StartNewSale => CashboxAction.StartNewSale,
            CashboxButton.FinishSale => CashboxAction.FinishSale,
            CashboxButton.PayWithCash => CashboxAction.PayWithCash,
            CashboxButton.PayWithCard => CashboxAction.PayWithCard,
            CashboxButton.DisableExpressMode => CashboxAction.DisableExpressMode,
            _ => throw new ArgumentOutOfRangeException(nameof(silaButton), silaButton, null)
        };
    }
}