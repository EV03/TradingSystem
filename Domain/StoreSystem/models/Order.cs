namespace Domain.StoreSystem.models;

public class Order
{
    public Guid Id { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? OrderingDate { get; set; }
    public String Status { get; set; } // incoming or arrived ( enum)
    
    public Guid StoreId { get; set; } // foreign key 
    public Store Store { get; set; } // navigation property
    
    public List<OrderItem> OrderItems { get; set; } // navigation property
}

