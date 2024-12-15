using Domain.CashDesk;
using Tecan.Sila2;

namespace CashDeskHardwareControllers.BarcodeScannerService;

public class SilaBarcodeScannerAdapter : IBarcodeScannerController
{
    private readonly IBarcodeScannerService _barcodeScannerService;
    
    private readonly IIntermediateObservableCommand<string>? _barcodeStream;
    
    
    public SilaBarcodeScannerAdapter(IBarcodeScannerService barcodeScannerService)
    {
        _barcodeScannerService = barcodeScannerService;
    }
    

    public Task<string>  ScanBarcode()
    {
        throw new NotImplementedException();
    }
}   