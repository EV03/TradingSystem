using Domain.CashDesk;
using Tecan.Sila2;

namespace CashDeskHardwareControllers.BarcodeScannerService;

public class SilaBarcodeScannerAdapter : IBarcodeScannerController
{
    private readonly IBarcodeScannerService _barcodeScannerService;
    
    private readonly IIntermediateObservableCommand<string>? _barcodeStream;
    
    public event EventHandler<string>? BarcodeScanned;
    
    
    public SilaBarcodeScannerAdapter(IBarcodeScannerService barcodeScannerService)
    {
        _barcodeScannerService = barcodeScannerService;
    }

    public void StartListeningToBarcodes()
    {
        throw new NotImplementedException();
    }

    public void StopListeningToBarcodes()
    {
        throw new NotImplementedException();
    }
}   