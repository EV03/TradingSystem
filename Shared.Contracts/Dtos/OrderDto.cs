namespace Shared.Contracts.Dtos;

public class OrderDto
{
    public long Id { get; set; } 
    public List<OrderSupplierDto> OrderSupplier { get; set; }
}