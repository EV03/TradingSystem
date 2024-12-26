namespace Domain.CashDesk;

public class BankTransactionContext
{
    public string Id { get; set; }
    
    public string Challenge { get; set; }
    
    public double Amount { get; set; }
    
    public DateTime Timestamp { get; set; }
    
    public string Status { get; set; }
    
    public BankTransactionContext(string id, string challenge, double amount, DateTime timestamp, string status)
    {
        Id = id;
        Challenge = challenge;
        Amount = amount;
        Timestamp = timestamp;
        Status = status;
    }
    
}