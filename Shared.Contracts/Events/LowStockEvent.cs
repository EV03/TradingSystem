namespace Shared.Contracts.Events;

public class LowStockEvent : IStoreEvent
{
    public long EnterpriseId { get; set; }
    public long ToStoreId { get; set; }
    public required List<long> ProductIds { get; set; }
}
