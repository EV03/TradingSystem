namespace Shared.Contracts.Events;

public class LowStockEvent : IStoreEvent
{
    public long EnterpriseId { get; set; }
    public long StoreId { get; set; }
    public long ProductId { get; set; }
    public long Quantity { get; set; }
}
