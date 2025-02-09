namespace Shared.Contracts.Dtos;

public class OrderDto
{
    public Guid Id { get; set; } 
    public List<OrderSupplierDto> OrderSupplier { get; set; }
}