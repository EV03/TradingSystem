
namespace CashDeskHardwareControllers.DisplayService;

public class SilaDisplayAdapter : Domain.CashDesk.IDisplayController
{
    
    private readonly IDisplayController _displayService;
    
    public SilaDisplayAdapter(IDisplayController displayService)
    {
        _displayService = displayService;
    }
    
    public void DisplayText(string displayText)
    {
        throw new NotImplementedException();
    }
}