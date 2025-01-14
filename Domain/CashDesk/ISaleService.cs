namespace Domain.CashDesk;

public interface ISaleService
{
    void StartSale();
    Task AddProductToSale(string barcode); 
    int GetSaleTotal();
    Task FinishSaleAsync();
    
    bool IsValidBarcode(string barcode);
}
