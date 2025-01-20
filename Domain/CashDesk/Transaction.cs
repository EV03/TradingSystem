namespace Domain.CashDesk;

public class Transaction
{
    List<SaleItem> SaleItems { get; set; }
    
    string paymentMehod { get; set; }
    
    DateTime TimeStamp { get; set; }
    
    public Transaction(List<SaleItem> saleItems, string paymentMethod)
    {
        SaleItems = saleItems;
        paymentMehod = paymentMethod;
        TimeStamp = DateTime.Now;
    }
}