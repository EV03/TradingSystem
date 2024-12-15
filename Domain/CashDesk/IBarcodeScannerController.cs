namespace Domain.CashDesk;

public interface IBarcodeScannerController
{
    Task<string> ScanBarcode();  
}