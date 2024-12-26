
using Domain.CashDesk;


namespace CashDeskHardwareControllers.PrinterService;

public class SilaPrinterAdapter : IPrinterController
{
    
    private readonly IPrintingService _printingService;
    
    public SilaPrinterAdapter(IPrintingService printingService)
    {
        _printingService = printingService;
    }
    public void Print(string content)
    {
        _printingService.PrintLine(content);
    }
}