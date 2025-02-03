namespace Shared.Contracts.Dtos;

public class OrderDto
{
    public Guid? Id { get; set; }
    List<OrderSupplierDto> OrderSupplier { get; set; }
}