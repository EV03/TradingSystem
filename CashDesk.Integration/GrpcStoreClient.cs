using Shared.Contracts;
using Shared.Contracts.Dtos;
using Shared.Contracts.Interfaces;

namespace CashDesk.Integration;

public class GrpcStoreClient : IStoreCommunication
{
    public Task<ProductDto> GetProduct(string barcode)
    {
        throw new NotImplementedException();
    }

    public Task FinishSale(TransactionDto transaction)
    {
        throw new NotImplementedException();
    }
}