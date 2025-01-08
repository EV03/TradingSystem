using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices.JavaScript;

namespace Domain.CashDesk;

public interface IBarcodeScannerController
{
    event EventHandler<string> BarcodeScanned;
    event EventHandler<string> BarcodeScanningFailed;
    void StartListeningToBarcodes();

    void StopListeningToBarcodes();
}