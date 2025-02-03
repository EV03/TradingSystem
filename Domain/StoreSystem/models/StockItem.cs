namespace Domain.StoreSystem.models;

public class StockItem
{
    public Guid Id { get; set; }
    public float  SalesPrice { get; set; } // das hier wird entfernt und in cachedProduct gespeichert
    public string Name { get; set; }
    public string Barcode { get; set; }  // das hier wird entfernt und in cachedProduct gespeichert
    
    int MinStock { get; set; } // for low stock alert
    public int AvailableQuantity { get; set; }
    public int OutGoingQuantity  { get; set; } // quantity sent to another store ( unavailable )
    public int IncomingQuantity { get; set; } // quantity received from another store ( unavailable )
    
    public Guid CachedProductId { get; set; }
    public CachedProduct CachedProduct { get; set; } // navigation property
}