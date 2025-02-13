namespace Shared.Contracts.Dtos;

public class StockDto
{
    public long Id { get; set; }
    public long CachedProductId { get; set; }
    public string Name { get; set; }
    public int AvailableQuantity { get; set; }
    public int IncomingQuantity { get; set; }
    public int OutGoingQuantity { get; set; }
}