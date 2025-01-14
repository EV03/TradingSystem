using Domain.CashDesk;

namespace CashDesk.Integration.Bank;

public class CardAuthorization : ICardReaderResult
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