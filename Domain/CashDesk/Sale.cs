namespace Domain.CashDesk;

public class Sale
{
    public List<SaleItem> Items { get; }
    public int Total { get; private set; }
    
    public Sale()
    {
        Items = new List<SaleItem>();
        Total = 0; 
    }
    
    public void AddItem(SaleItem item)
    {
        var existingItem = Items.FirstOrDefault(i => i.Barcode == item.Barcode);
        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            Items.Add(item);
        }
        CalculateTotal(item.Price);
    }

    private void CalculateTotal(int itemPrice)
    {
        Total += itemPrice;
    }
    
    public bool IsEmpty()
    {
        return Items.Count == 0;
    }
}