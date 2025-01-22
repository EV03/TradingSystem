namespace Domain.CashDesk;

public interface ITransactionRepository
{
    void SaveTransaction(Transaction transaction);
    
    IEnumerable<Transaction> GetRecentTransactions();
    IEnumerable<Transaction> GetAllTransactions();
    void DeleteTransactions();
}