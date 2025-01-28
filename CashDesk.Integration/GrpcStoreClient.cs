using Shared.Contracts;
using Shared.Contracts.Dtos;
using Shared.Contracts.Interfaces;

namespace CashDesk.Integration;

// Todo : sobald store grpc service implementiert ist, wird dieser client angepasst
public class GrpcStoreClient : IStoreCommunication
{
    public Task<ProductDto> GetProduct(string barcode)
    {
       // erstmal nur ein dummy bis grpc client implementiert ist
       
         return Task.FromResult(new ProductDto
         {
              Barcode = barcode,
              Name = "Dummy Product",
              Price = 150,
         });
    }

    public Task UpdateInventory(TransactionDto transaction)
    {
        // erstmal nur ein dummy bis grpc client implementiert ist
        return Task.CompletedTask;
    }
}