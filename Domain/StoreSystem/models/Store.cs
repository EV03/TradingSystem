using Domain.StoreSystem.models;

namespace Domain.StoreSystem.models;

public class Store
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public List<Order> Orders { get; set; } // navigation property
    public List<StockItem> StockItems { get; set; } // navigation property
    
    public Guid EnterpriseId { get; set; } // foreign key
   
 
}