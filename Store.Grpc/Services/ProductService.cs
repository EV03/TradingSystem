using Domain.StoreSystem.repository;
using Grpc.Core;
using Shared.Contracts;
using Shared.Contracts.Protos;

namespace Store.Grpc.Services
{
    public class ProductService : Product.ProductBase
    {
            private readonly IStockItemRepository _stockItemRepository;

            public ProductService(IStockItemRepository stockItemRepository)
            {
                _stockItemRepository = stockItemRepository;
            }
            public override async Task<ProductResponse> GetProductInfo(ProductRequest request, ServerCallContext context)
            {
                var product = await _stockItemRepository.GetByBarcodeAsync(request.Barcode);

                if (product == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
                }

                return new ProductResponse
                {
                    Name = product.Name,
                    Barcode = product.Barcode,
                    Price = product.SalesPrice,
                };
            }
        }
}