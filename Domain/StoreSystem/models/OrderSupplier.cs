using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.StoreSystem.models;

public class OrderSupplier
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? OrderDate { get; set; }
    
    public long SupplierId { get; set; }
    public long OrderId { get; set; }
    public Order Order { get; set; }
    
    // n zu m beziehung zu cachedProducts
    public  List<OrderSupplierCachedProduct> OrderSupplierProducts { get; set; }
    
}