using Google.Protobuf.Collections;
using Grpc.Net.Client;
using Shared.Contracts.Dtos;
using Shared.Contracts.Interfaces;
using Shared.Contracts.Protos;


namespace CashDesk.Integration;

public class GrpcStoreClient : IStoreCommunication
{
    private readonly Product.ProductClient _productServiceClient;
    public GrpcStoreClient()
    {
        var channel = GrpcChannel.ForAddress("https://localhost:5131"); 
        _productServiceClient = new Product.ProductClient(channel);
    }
    // grpc client stub 
    public async Task<ProductDto> GetProduct(string barcode)
    {
        var request = new ProductRequest() { Barcode = barcode };
        
        var response = await _productServiceClient.GetProductInfoAsync(request);
        
        return new ProductDto
        {
            Name = response.Name,
            Barcode = response.Barcode,
            Price = (int) response.Price * 100
        };
    }

    public async  Task UpdateInventory(TransactionDto transaction)
    {
        var request = new UpdateInventoryRequest
        {
            Items =
            {
                transaction.Items.Select(item => new TransactionItem
                {
                    Barcode = item.Key,
                    Quantity = item.Value 
                })
            }
        };
        
       var result =  await _productServiceClient.UpdateInventoryAsync(request);
       
       /* retry logic can be added here 
         if (!result.Success)
         {
              throw new Exception("Failed to update inventory");
         }
         */
    }
}