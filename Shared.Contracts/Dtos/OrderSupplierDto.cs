namespace Shared.Contracts.Dtos;

public class OrderSupplierDto
{
    public Guid? Id { get; set; }
    public Guid? SupplierId { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    List<OrderSupplierProductDto> OrderSupplierProducts { get; set; }
}