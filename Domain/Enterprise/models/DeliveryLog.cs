namespace Domain.Enterprise.models;

public class DeliveryLog
{
    public long Id { get; set; }
    public  String SupplierName { get; set; }
    public long SupplierId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime DeliveryDate { get; set; }
} 