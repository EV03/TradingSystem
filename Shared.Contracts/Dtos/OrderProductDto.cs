namespace Shared.Contracts.Dtos;

public class OrderProductDto
{
    public Guid CachedProductId { get; set; }
    public int Quantity { get; set; }
}