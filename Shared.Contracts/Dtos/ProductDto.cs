namespace Shared.Contracts.Dtos;

public class ProductDto
{
    public string Barcode { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
     
    public Guid? ProductId { get; set; }
    
}