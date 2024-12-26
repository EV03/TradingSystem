namespace Domain.CashDesk;

public interface IBarcodeScannerController
{
    event EventHandler<string> BarcodeScanned;
    void StartListeningToBarcodes();

    void StopListeningToBarcodes();

}

