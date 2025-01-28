using Domain.StoreSystem.models;

namespace Domain.Enterprise.models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; } 
    public string Barcode { get; set; }
    
    public static Guid SupplierId { get; set; }
    public Supplier Supplier { get; set; } // navigation property
    public List<StockItem> StockItems { get; set; } // navigation property
    
}