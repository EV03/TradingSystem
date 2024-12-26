using Domain.CashDesk;
using Shared.Contracts;

namespace CashDesk.Application;

public class SaleService : ISaleService
{
    public Task StartSaleAsync()
    {
        throw new NotImplementedException();
    }
    

    public Task AddProductToSale(string barcode)
    {
        throw new NotImplementedException();
    }

    public Task<double> GetSaleTotalAsync()
    {
        throw new NotImplementedException();
    }

    public Task FinishTSaleAsync()
    {
        throw new NotImplementedException();
    }
}