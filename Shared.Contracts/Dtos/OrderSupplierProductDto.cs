namespace Shared.Contracts.Dtos;

public class OrderSupplierProductDto
{
    public Guid? SupplierId { get; set; }
    public Guid CachedProductId { get; set; }
    public int Quantity { get; set; }
}