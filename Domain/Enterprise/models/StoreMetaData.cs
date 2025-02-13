namespace Domain.Enterprise.models;

public class StoreMetaData
{
    public Guid Id { get; set; }
    Guid StoreId { get; set; } 
    string Name { get; set; }
    string Location { get; set; }
}