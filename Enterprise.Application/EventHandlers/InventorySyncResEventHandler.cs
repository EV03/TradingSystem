using Domain.StoreSystem.repository;
using Shared.Contracts.Events;

namespace Enterprise.Application.EventHandlers;

public class InventorySyncResEventHandler : IEventHandler<InventorySyncResEvent>
{
    private readonly IEventBus _eventBus; 
    private readonly int _totalStoreCount;
    private readonly List<InventorySyncResEvent> _receivedEvents = new();
    
    public InventorySyncResEventHandler(IEventBus eventBus, int totalStoreCount)
    {
        _eventBus = eventBus;
        _totalStoreCount = totalStoreCount;
    }
    public async Task HandleAsync(InventorySyncResEvent @event)
    {
        _receivedEvents.Add(@event);
        
        int expectedStoreCount = _totalStoreCount - @event.ExcludedStoreIds.Count;
        
        // wait for all stores to respond
        if(expectedStoreCount == _receivedEvents.Count)
        {
            var allProductsStock = _receivedEvents.SelectMany(e => e.ProductsStock).ToList();

            var productTransferDetails = new List<ProductTransferDetail>();

            foreach (var storeProductStock in allProductsStock.GroupBy(p => p.ProductId))
            {
                var maxStockitem = storeProductStock.OrderByDescending(x => x.Quantity).FirstOrDefault();
                
                var maxStockItem = storeProductStock.OrderByDescending(p => p.Quantity).FirstOrDefault();
                if (maxStockItem != null && maxStockItem.Quantity >= 2 * maxStockItem.minStock)
                {
                    productTransferDetails.Add(new ProductTransferDetail
                    {
                        ProductId = maxStockItem.ProductId,
                        FromStoreId = maxStockItem.FromStoreId,
                        Quantity = maxStockItem.Quantity / 3 // transfer 1/3 of the stock
                    });
                }
            }
            
            if (productTransferDetails.Any())
            {
                var productTransferEvent = new ProductTransferEvent
                {
                    EnterpriseId = @event.EnterpriseId,
                    ToStoreId = @event.ToStoreId,
                    ProductTransferDetails = productTransferDetails
                };
                await _eventBus.PublishAsync(productTransferEvent);
            }
        }
    }
}