namespace Shared.Contracts.Dtos;

public class StockDto
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public int AvailableQuantity { get; set; }
    public int IncomingQuantity { get; set; }
}