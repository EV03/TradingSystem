using Domain.CashDesk;
using Tecan.Sila2;

namespace CashDeskHardwareControllers.CashBoxService;

public class SilaCashBoxAdapter : ICashBoxController
{
    private readonly ICashboxService _cashboxService;
    private readonly ISaleService _saleService;
    private IIntermediateObservableCommand<CashboxButton>? _buttonStream;
    
    public SilaCashBoxAdapter(ICashboxService cashboxService, ISaleService saleService)
    {
        _cashboxService = cashboxService;
        _saleService = saleService;
    }

    public event EventHandler<CashDeskAction>? ActionTriggered;
    public event EventHandler<string>? ListeningFailed;

    public void StartListeningToCashbox()
    {
        if (_buttonStream != null)
        {
            throw new InvalidOperationException("Already listening to cashbox buttons.");
        }

        try
        {
            _buttonStream = _cashboxService.ListenToCashdeskButtons();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while listening to Cashbox: {ex.Message}");
            ListeningFailed?.Invoke(this, ex.Message);
        }
        
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error while listening to cashbox buttons: {ex.Message}");
                ListeningFailed?.Invoke(this, ex.Message);
            }
        });
    }

    public void StopListeningToCashbox()
    {
        if (_buttonStream == null)
        {
            throw new InvalidOperationException("Not listening to cashbox buttons.");
        }

        _buttonStream.Cancel();
        _buttonStream = null;
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