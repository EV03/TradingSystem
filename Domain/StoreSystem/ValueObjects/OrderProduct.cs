namespace Domain.StoreSystem.ValueObjects;

public class OrderProduct
{
    public Guid CachedProductId { get; set; }
    
    public int Quantity { get; set; }
}