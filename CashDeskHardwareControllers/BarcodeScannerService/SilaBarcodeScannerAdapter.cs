using Domain.CashDesk;
using Tecan.Sila2;

namespace CashDeskHardwareControllers.BarcodeScannerService;

public class SilaBarcodeScannerAdapter : IBarcodeScannerController
{
    private readonly IBarcodeScannerService _barcodeScannerService;
    
    private  IIntermediateObservableCommand<string>? _barcodeStream;
    
    public event EventHandler<string>? BarcodeScanned;
    public event EventHandler<string>? BarcodeScanningFailed;


    public SilaBarcodeScannerAdapter(IBarcodeScannerService barcodeScannerService)
    {
        _barcodeScannerService = barcodeScannerService;
    }

    public void StartListeningToBarcodes()
    {
       if( _barcodeStream != null)
       {
           throw new InvalidOperationException("Already listening to barcodes.");
       }
       _barcodeStream = _barcodeScannerService.ListenToBarcodes();
       
         Task.Run(async () =>
         {
              try
              {
                while (await _barcodeStream.IntermediateValues.WaitToReadAsync())
                {
                     if (_barcodeStream.IntermediateValues.TryRead(out var barcode))
                     {
                          BarcodeScanned?.Invoke(this, barcode);
                     }
                }
              }
              catch (Exception ex)
              {
                Console.WriteLine($"Error while listening to barcodes: {ex.Message}");
                BarcodeScanningFailed?.Invoke(this, ex.Message);
              }
         });
    }

    public void StopListeningToBarcodes()
    {
        if(_barcodeStream == null)
        {
            throw new InvalidOperationException("Not listening to barcodes.");
        }
        _barcodeStream.Cancel();
        _barcodeStream = null;
    }
}   