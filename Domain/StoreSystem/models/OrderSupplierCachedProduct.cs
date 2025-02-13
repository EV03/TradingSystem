using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.StoreSystem.models;

public class OrderSupplierCachedProduct
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public int Quantity { get; set; }
    
    public long OrderSupplierId { get; set; }
    public OrderSupplier OrderSupplier { get; set; }
    
    public long CachedProductId { get; set; }
    public CachedProduct CachedProduct { get; set; }
    
}