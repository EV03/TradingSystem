namespace Domain.Enterprise.models;

public class DeliveryLog
{
    public Guid EnterpriseId { get; set; }  
    
    public  String SupplierName { get; set; }
    public Guid SupplierId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime DeliveryDate { get; set; }
} 