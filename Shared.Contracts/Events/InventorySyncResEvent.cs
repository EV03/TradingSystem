namespace Shared.Contracts.Events;

public class  InventorySyncResEvent : IStoreEvent 
{
    public long EnterpriseId { get; set; }
    public long ToStoreId { get; set; }
    public required  List<long> ExcludedStoreIds { get; set; }
    public required List<InventoryResponseProductsStock> ProductsStock{ get; set; }
}
public class InventoryResponseProductsStock
{ 
    public long ProductId { get; set; }
    public long minStock { get; set; }
    public  long FromStoreId { get; set; }
    public int Quantity { get; set; }
}