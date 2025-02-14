using Domain.StoreSystem.models;
using Domain.StoreSystem.repository;
using Shared.Contracts.Dtos;
using Shared.Contracts.Events;
using Store.Application.service;

namespace Store.Application;

public class StockService : IStockService
{
    private readonly IStockItemRepository _stockItemRepository;
    private readonly IProductRepository _productRepository;
    private readonly IEventBus _eventBus;
    private readonly string _enterpriseId;
    private readonly string _storeId;
    
    public StockService(IStockItemRepository stockItemRepository,IProductRepository productRepository, IEventBus eventBus)
    {
        _stockItemRepository = stockItemRepository;
        _productRepository = productRepository;
        _eventBus = eventBus;
        _enterpriseId = Environment.GetEnvironmentVariable("ENTERPRISE_ID") ?? throw new Exception("ENTERPRISE_ID is not set");
        _storeId = Environment.GetEnvironmentVariable("STORE_ID") ?? throw new Exception("STORE_ID is not set");
    }
    public async Task<List<StockItem>> GetStockReportAsync()
    {
        var stockItems = await _stockItemRepository.GetAllStocksAsync();
        if(stockItems == null)
        {
            throw new Exception("No stock items found");
        }  

        return stockItems.ToList();
    }

    public async Task UpdateStockFromSaleAsync(TransactionDto saleItems)
    {
        LowStockEvent lowStockEvent = null;
        
        foreach (var saleItem in saleItems.Items)
        {
            var product = await _productRepository.GetByBarcodeAsync(saleItem.Key);
            if(product == null)
            {
                throw new Exception("Product not found");
            }
            var stockItem = await _stockItemRepository.GetByIdAsync(product.StockItem.Id);
            if(stockItem == null)
            {
                throw new Exception("Stock item not found");
            }
            stockItem.AvailableQuantity -= saleItem.Value;
            
            if(stockItem.MinStock > stockItem.AvailableQuantity && lowStockEvent == null)
            {
                lowStockEvent = new LowStockEvent
                {
                    EnterpriseId = long.Parse(_enterpriseId),
                    ProductIds = new List<long> {product.Id},
                    ToStoreId  = long.Parse(_storeId)
                };
            }
            else if(stockItem.MinStock > stockItem.AvailableQuantity)
            {
                lowStockEvent?.ProductIds.Add(product.Id);
            }
            await _stockItemRepository.UpdateAsync(stockItem);
        }
        if(lowStockEvent != null)
        {
            await _eventBus.PublishAsync(lowStockEvent);
        }
    }

    public async Task UpdateStockFromOrderAsync(OrderSupplier orderSupplier, bool isIncoming)
    {
         foreach (var orderSupplierProduct in orderSupplier.OrderSupplierProducts)
         { 
             var product = await _productRepository.GetByProductIdAsync(orderSupplierProduct.CachedProduct.ProductId);
             if(product == null)
             {
                    throw new Exception("Product not found");
             }
                
             var stockItem = await _stockItemRepository.GetByIdAsync(product.StockItem.Id);
             if(stockItem == null)
             {
                    throw new Exception("Stock item not found");
             }
             
             if(isIncoming)
             {
                    stockItem.IncomingQuantity += orderSupplierProduct.Quantity;
             }
             else
             {
                    stockItem.AvailableQuantity += orderSupplierProduct.Quantity;
                    stockItem.IncomingQuantity -= orderSupplierProduct.Quantity;
             }
             
             await _stockItemRepository.UpdateAsync(stockItem);
         }
    }
}