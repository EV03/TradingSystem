namespace Domain.CashDesk;

public interface ISaleService
{
    void StartSale();
    Task AddProductToSale(string barcode); 
    long GetSaleTotal();
    Task FinishSaleAsync();
    
    bool IsValidBarcode(string barcode);
}
