namespace Domain.StoreSystem.models;

public class StockItem
{
    public long Id { get; set; }
    public string Name { get; set; }
    
    int MinStock { get; set; } // for low stock alert
    public int AvailableQuantity { get; set; }
    public int OutGoingQuantity  { get; set; } // quantity sent to another store ( unavailable )
    public int IncomingQuantity { get; set; } // quantity received from another store ( unavailable )
    public long CachedProductId { get; set; }
    public CachedProduct CachedProduct { get; set; } // navigation property
}