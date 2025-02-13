namespace Shared.Contracts.Events;

public class OrderDeliveredEvent
{
    public long OrderId { get; init; }
    public long SupplierId { get; init; }
    public long EnterpriseId { get; set; }
    public DateTime DeliveryDate { get; init; }
}