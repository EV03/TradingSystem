using Domain.StoreSystem;

namespace Domain.Enterprise.models;

public class EnterpriseSupplier
{
    public Guid Id { get; set; }
    public Guid EnterpriseId { get; set; }
    public Models.Enterprise Enterprise { get; set; }
    
    public Guid SupplierId { get; set; }
    public Supplier Supplier { get; set; }
}