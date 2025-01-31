namespace Domain.Enterprise.models;

public class DeliveryLog
{
    Guid SupplierId { get; set; }
    DateTime OrderDate { get; set; }
    DateTime DeliveryDate { get; set; }
} 