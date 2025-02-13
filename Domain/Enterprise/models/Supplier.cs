using Domain.Enterprise.models;
using Domain.StoreSystem;

namespace Domain.Enterprise.models;

public class Supplier
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<Product> Products { get; set; } // navigation property
}