namespace Shared.Contracts.Dtos;

public class OrderDto
{
    public Guid StoreId { get; set; }
    public List<OrderItemDto> OrderItems { get; set; }
}