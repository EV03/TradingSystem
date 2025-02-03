namespace Domain.StoreSystem.models;

public class Order
{
    public Guid Id { get; set; }
    public List<OrderSupplier> OrderSupplier { get; set; }
    
}

