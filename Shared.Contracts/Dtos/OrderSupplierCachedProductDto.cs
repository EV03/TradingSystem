namespace Shared.Contracts.Dtos;

public class OrderSupplierCachedProductDto
{
    public long CachedProductId { get; set; }
    public int Quantity { get; set; }
    
    public string CachedProductName { get; set; }
}