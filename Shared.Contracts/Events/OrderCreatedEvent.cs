namespace Shared.Contracts.Events;

public class OrderCreatedEvent : IStoreEvent
{
    public Guid OrderId { get; init; }
    public Guid SupplierId { get; init; }
    public string? SupplierName { get; init; }
    public int EnterpriseId { get; set; }
    public DateTime OrderDate { get; init; }
}
