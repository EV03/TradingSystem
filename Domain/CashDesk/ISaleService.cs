namespace Domain.CashDesk;

public interface ISaleService
{
    void StartSaleAsync();
    Task<OperationResult> AddProductToSale(string barcode); 
    int GetSaleTotalAsync();
    Task<OperationResult> FinishSaleAsync(Transaction transaction);
}
