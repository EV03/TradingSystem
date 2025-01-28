using Domain.Enterprise.models;
using Domain.StoreSystem;

namespace Domain.Enterprise.models;

public class Supplier
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public List<Product> Products { get; set; } // navigation property
    public List<EnterpriseSupplier> EnterpriseSupplier { get; set; } // navigation property
}