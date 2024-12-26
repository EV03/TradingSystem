namespace Domain.CashDesk;

public interface ISaleService
{
    Task StartSaleAsync();
    Task AddProductToSale(string barcode);
    
    Task<double> GetSaleTotalAsync();
    
    Task FinishTSaleAsync();
}