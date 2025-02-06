using Domain.StoreSystem.models;
using Domain.StoreSystem.repository;
using Shared.Contracts.Dtos;
using Store.Application.service;

namespace Store.Application;

public class StockService : IStockService
{
    private readonly IStockItemRepository _stockItemRepository;
    private readonly IProductRepository _productRepository;
    
    public StockService(IStockItemRepository stockItemRepository,IProductRepository productRepository)
    {
        _stockItemRepository = stockItemRepository;
        _productRepository = productRepository;    
        
    }
    public async  Task<List<StockItem>> GetStockReportAsync()
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
            await _stockItemRepository.UpdateAsync(stockItem);
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