
namespace CashDeskHardwareControllers.DisplayService;

public class SilaDisplayAdapter : Domain.CashDesk.IDisplayController
{
    
    private readonly IDisplayService _displayService;
    
    public SilaDisplayAdapter(IDisplayService displayService)
    {
        _displayService = displayService;
    }
    
    public void DisplayText(string displayText)
    {
        _displayService.SetDisplayText(displayText);
    }
}