namespace Domain.CashDesk;

public interface ICashBoxController
{
    event EventHandler<CashboxAction> ActionTriggered;

     void StartListeningToCashbox();
     
}
 
 