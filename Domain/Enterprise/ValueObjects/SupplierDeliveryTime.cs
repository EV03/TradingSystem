namespace Domain.Enterprise.ValueObjects;

public class SupplierDeliveryTime
{
    public Guid SupplierId { get; set; }
    
    public int AverageDeliveryTime { get; set; }
}