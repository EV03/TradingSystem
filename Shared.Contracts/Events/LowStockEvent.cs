namespace Shared.Contracts.Events;

public class LowStockEvent : IStoreEvent
{
    public int EnterpriseId { get; set; }
    public int StoreId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
