namespace Domain.CashDesk;

public interface ICashBoxController
{
    event EventHandler<CashDeskAction> ActionTriggered;

     void startCashbox();
     
}
 
 