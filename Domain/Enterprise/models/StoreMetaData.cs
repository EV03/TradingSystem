namespace Domain.Enterprise.models;

public class StoreMetaData
{
    Guid StoreId { get; set; } 
    
    Guid EnterpriseId { get; set; }
    
    string Name { get; set; }
    
    string Location { get; set; }
}