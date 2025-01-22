namespace Domain.CashDesk;

public class Transaction
{
   public List<SaleItem> SaleItems { get; private set; }
    
   public string PaymentMehod { get; private set; }

   public DateTime Timestamp { get; private set; }
    
    public Transaction(List<SaleItem> saleItems, string paymentMethod)
    {
        SaleItems = saleItems;
        PaymentMehod = paymentMethod;
        Timestamp = DateTime.Now;
    }
}