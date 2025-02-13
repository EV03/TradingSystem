namespace Shared.Contracts.Events;

// Stores response to the InventorySyncEvent
public class StockUpdateEvent : IStoreEvent
{
    public long EnterpriseId { get; set; }
}