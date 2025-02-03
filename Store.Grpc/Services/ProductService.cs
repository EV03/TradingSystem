using Domain.StoreSystem.repository;
using Grpc.Core;
using Shared.Contracts.Dtos;
using Shared.Contracts.Protos;
using Store.Application.service;

namespace Store.Grpc.Services
{
    public class ProductService : Product.ProductBase
    {
            private readonly IProductService  _productService;
            private readonly IStockService _stockService;

            public ProductService(IProductService productService, IStockService stockItemService)
            {
                _productService = productService;
                _stockService = stockItemService;
            }
        
            public override async Task<ProductResponse> GetProductInfo(ProductRequest request, ServerCallContext context)
            {
                try
                {
                    var product = await _productService.ShowProductDetails(request.Barcode);
                    
                    return new ProductResponse
                    {
                        Name = product.Name,
                        Barcode = product.Barcode,
                        Price = product.CurrentPrice,
                    };
                }
                catch (ArgumentException e)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, e.Message));
                }
            }
            
            public override async Task<UpdateInventoryResponse> UpdateInventory(UpdateInventoryRequest request, ServerCallContext context)
            {
                var transactionDto = new TransactionDto
                {
                    Items = new Dictionary<string, int>()
                };
                foreach (var item in request.Items)
                { 
                    transactionDto.Items.Add(item.Barcode, item.Quantity);
                }
                try
                {
                    await _stockService.UpdateStockFromSaleAsync(transactionDto);

                    return new UpdateInventoryResponse
                    {
                        Success = true
                    };
                }
                catch ( ArgumentException e)
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, e.Message));
                }
            }
        }
}