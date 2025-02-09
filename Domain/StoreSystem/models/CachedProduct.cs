namespace Domain.StoreSystem.models;

public class CachedProduct
{
   public Guid Id { get; set; }
   public String Barcode { get; set; }
   public String Name { get; set; }
   public double CurrentPrice { get; set; }
   
   public Guid ProductId { get; set; }
   public Guid SupplierId { get; set; } // eventuell rausnehmen 
   
   public StockItem StockItem { get; set; } // navigation property
    
}