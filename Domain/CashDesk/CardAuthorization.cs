namespace Domain.CashDesk;

public class CardAuthorization
{
   public string Account { get; set; }
    
   public  string Token { get; set; }
    
   public int Amount { get; set; }
   
   public CardAuthorization(string account, string token, int amount)
   {
       Account = account;
       Token = token;
       Amount = amount;
   }
    
}