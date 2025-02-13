namespace Domain.Enterprise.models;

public class StoreMetaData
{
    public long Id { get; set; }
    long StoreId { get; set; } 
    string Name { get; set; }
    string Location { get; set; }
}