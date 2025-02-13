namespace Domain.StoreSystem.models;

public class CachedProduct
{
   public long Id { get; set; }
   public String Barcode { get; set; }
   public String Name { get; set; }
   public double CurrentPrice { get; set; }
   
   public long ProductId { get; set; }
   public long SupplierId { get; set; }

   public StockItem StockItem { get; set; } // navigation property
   
   public List<OrderSupplierCachedProduct> OrderSupplierProducts { get; set; } // navigation property
    
}