namespace Domain.StoreSystem.ValueObjects;

public class OrderProduct
{
    public Guid ProductId { get; set; }
    
    public int Quantity { get; set; }
}