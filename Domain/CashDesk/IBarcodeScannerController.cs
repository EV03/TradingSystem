namespace Domain.CashDesk;

public interface IBarcodeScannerController
{
    event EventHandler<string> BarcodeScanned;
    event EventHandler<string> BarcodeScanningFailed;
    void StartListeningToBarcodes();

    void StopListeningToBarcodes();

}

