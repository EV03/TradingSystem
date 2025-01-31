namespace Domain.StoreSystem.models;

public class OrderSupplierCachedProduct
{
    public Guid Id { get; set; }
    public Guid SupplierId { get; set; }
    
    public Guid OrderSupplierId { get; set; }
    public OrderSupplier OrderSupplier { get; set; }
    
    public Guid CachedProductId { get; set; }
    public CachedProduct CachedProduct { get; set; }
    
}