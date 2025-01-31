namespace Domain.StoreSystem.models;

public class CachedProduct
{
   public Guid Id { get; set; }
   public String Barcode { get; set; }
   public String Name { get; set; }
   public float Currentprice { get; set; }
   
   public Guid StoreId { get; set; }
   public Store Store { get; set; } // navigation property
   
   public Guid ProductId { get; set; }
   public Guid SupplierId { get; set; } // eventuell rausnehmen 
   
   public StockItem StockItem { get; set; } // navigation property
    
}