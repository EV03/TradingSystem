namespace Shared.Contracts.Events;

public class OrderCreatedEvent : IStoreEvent
{
    public long OrderId { get; init; }
    public long OrderSupplierId { get; init; }
    public long SupplierId { get; init; }
    public string? SupplierName { get; init; }
    public long EnterpriseId { get; set; }
    public DateTime OrderDate { get; init; }
}
