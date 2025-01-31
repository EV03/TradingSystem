namespace Domain.StoreSystem.models;

public class Order
{
    public Guid Id { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? OrderDate { get; set; }
    public String Status { get; set; } // incoming or arrived ( enum)
    
    public List<OrderSupplier> OrderSupplier { get; set; }
    
    public Guid StoreId { get; set; } // foreign key 
    public Store Store { get; set; } // navigation property
    
}

