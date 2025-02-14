using Domain.StoreSystem.repository;
using Shared.Contracts.Events;
using Store.Application.service;

namespace Store.Application.EventHandlers;

public class ProductTransferEventHandler : IEventHandler<ProductTransferEvent>
{
    private readonly string _storeId;
    private readonly IStockItemRepository  _stockItemRepository;
    
    public ProductTransferEventHandler(IStockItemRepository stockItemRepository)
    {
        _storeId = Environment.GetEnvironmentVariable("STORE_ID") ?? throw new Exception("STORE_ID is not set");
    }
    public async Task HandleAsync(ProductTransferEvent @event)
    {
        // if we are the fromstore then we need to remove the quantity from our stock
        // if we are the tostore then we need to add the quantity to our stock
        foreach (var productTransferDetail in @event.ProductTransferDetails)
        {
            if(productTransferDetail.FromStoreId == long.Parse(_storeId))
            { 
                var stockItem = await _stockItemRepository.GetByProductIdAsync(productTransferDetail.ProductId);
                if(stockItem == null)
                {
                    throw new Exception($"StockItem with ProductId {productTransferDetail.ProductId} not found");
                }
                stockItem.OutGoingQuantity += productTransferDetail.Quantity;
                stockItem.AvailableQuantity -= productTransferDetail.Quantity;
                await _stockItemRepository.UpdateAsync(stockItem);
            }
            
           else if(@event.ToStoreId == long.Parse(_storeId))
            {
                var stockItem = await _stockItemRepository.GetByProductIdAsync(productTransferDetail.ProductId);
                if(stockItem == null)
                {
                    throw new Exception($"StockItem with ProductId {productTransferDetail.ProductId} not found");
                }
                stockItem.IncomingQuantity += productTransferDetail.Quantity;
                await _stockItemRepository.UpdateAsync(stockItem);
            }
        }
    } 
}