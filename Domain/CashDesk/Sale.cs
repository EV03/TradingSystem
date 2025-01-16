namespace Domain.CashDesk;

public class Sale
{
    public List<SaleItem> Items { get; private set; }
    public long Total { get; private set; }
    
    public Sale()
    {
        Items = new List<SaleItem>();
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
        CalculateTotal();
    }

    private void CalculateTotal()
    {
        Total = Items.Sum(item => item.Price * item.Quantity);
    }
    
    public bool IsEmpty()
    {
        return Items.Count == 0;
    }
    

}