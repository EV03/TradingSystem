namespace Domain.StoreSystem.models;

public class StockItem
{
    Guid Id { get; set; }
    double SalesPrice { get; set; }
    int MinStock { get; set; } // for low stock alert
    public int AvailableQuantity { get; set; }
    public int OutGoingQuantity  { get; set; } // quantity sent to another store ( unavailable )
    
    public Guid ProductId { get; set; }
    public Guid StoreId { get; set; }
    public Store Store { get; set; } // navigation property
}