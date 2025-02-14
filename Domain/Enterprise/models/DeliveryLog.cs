using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Enterprise.models;

public class DeliveryLog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   
    public long Id { get; set; }
    public long OrderId { get; set; }
    public long OrderSupplierId { get; set; }
    public  String SupplierName { get; set; }
    public long SupplierId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
} 