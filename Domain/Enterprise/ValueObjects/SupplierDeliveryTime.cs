namespace Domain.Enterprise.ValueObjects;

public class SupplierDeliveryTime
{
    public long SupplierId { get; set; }
    
    public int AverageDeliveryTime { get; set; }
}