namespace Domain.StoreSystem.models;

public class StockItem
{
    public Guid Id { get; set; }
    public float  SalesPrice { get; set; }
    public string Name { get; set; }
    public string Barcode { get; set; }
    
    int MinStock { get; set; } // for low stock alert
    public long AvailableQuantity { get; set; }
    public long OutGoingQuantity  { get; set; } // quantity sent to another store ( unavailable )
    
    public Guid ProductId { get; set; }
    public Guid StoreId { get; set; }
    public Store Store { get; set; } // navigation property
}