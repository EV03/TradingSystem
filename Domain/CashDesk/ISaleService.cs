namespace Domain.CashDesk;

public interface ISaleService
{
    void StartSale();
    Task<OperationResult> AddProductToSale(string barcode); 
    int GetSaleTotal();
    Task<OperationResult> FinishSaleAsync();
    
    bool isValidBarcode(string barcode);
}
