namespace Domain.CashDesk;

public interface ITransactionService
{
    Task StartTransactionAsync();
    
    Task<Product> GetProductAsync(string barcode);
    
    Task AddProductToTransaction(string barcode);
    
    Task<double> GetTransactionTotalAsync();
    
    Task FinishTransactionAsync();
}