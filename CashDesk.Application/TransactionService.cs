using Domain.CashDesk;
using Shared.Contracts;

namespace CashDesk.Application;

public class TransactionService : ITransactionService
{
    public Task StartTransactionAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Product> GetProductAsync(string barcode)
    {
        throw new NotImplementedException();
    }

    public Task AddProductToTransaction(string barcode)
    {
        throw new NotImplementedException();
    }

    public Task<double> GetTransactionTotalAsync()
    {
        throw new NotImplementedException();
    }

    public Task FinishTransactionAsync()
    {
        throw new NotImplementedException();
    }
}