namespace Shared.Contracts.Dtos;

public class StockDto
{
    public Guid Id { get; set; }
    public Guid CachedProductId { get; set; }
    public string Name { get; set; }
    public int AvailableQuantity { get; set; }
    public int IncomingQuantity { get; set; }
    public int OutGoingQuantity { get; set; }
}