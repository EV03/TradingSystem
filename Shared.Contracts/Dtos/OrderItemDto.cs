namespace Shared.Contracts.Dtos;

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    
    public int Amount { get; set; }
}