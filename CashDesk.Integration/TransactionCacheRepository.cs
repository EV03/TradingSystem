using Domain.CashDesk;


namespace CashDesk.Integration;

public class TransactionCacheRepository : ITransactionRepository
{
    
    public void SaveTransaction(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Transaction> GetAllTransactions()
    {
        throw new NotImplementedException();
    }

    public void DeleteTransactions()
    {
        throw new NotImplementedException();
    }
}