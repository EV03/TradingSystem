namespace Domain.StoreSystem.models;

public class OrderSupplier
{
    Guid Id { get; set; }
    Guid SupplierId { get; set; }
    
    // n zu m beziehung zu cachedProducts
    List<OrderSupplierCachedProduct> OrderSupplierProducts { get; set; }
    
}