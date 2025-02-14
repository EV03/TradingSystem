using Shared.Contracts.Events;

namespace Enterprise.Application.EventHandlers;

public class LowStockEventHandler : IEventHandler<LowStockEvent>
{
    private readonly IEventBus _eventBus;
    
    public LowStockEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
    public async  Task HandleAsync(LowStockEvent @event)
    {
        // here we could check which stores we want to exclude by heuristics
        
        var inventorySyncReqEvent = new InventorySyncReqEvent()
        {
            EnterpriseId = @event.EnterpriseId,
            ProductIds = @event.ProductIds,
            ExcludedStoreIds = new List<long>() { @event.ToStoreId },
            ToStoreId = @event.ToStoreId
        };
       await _eventBus.PublishAsync(inventorySyncReqEvent);
    }
}