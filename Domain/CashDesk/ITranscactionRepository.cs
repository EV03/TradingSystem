namespace Domain.CashDesk;

public interface ITransactionRepository
{
    void SaveTransaction(Transaction transaction);
    IEnumerable<Transaction> GetAllTransactions();
    void DeleteTransactions();
}