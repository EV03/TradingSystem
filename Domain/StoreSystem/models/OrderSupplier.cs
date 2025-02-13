namespace Domain.StoreSystem.models;

public class OrderSupplier
{
    public Guid Id { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public DateTime? OrderDate { get; set; }
    
    public Guid SupplierId { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    
    // n zu m beziehung zu cachedProducts
    public  List<OrderSupplierCachedProduct> OrderSupplierProducts { get; set; }
    
}