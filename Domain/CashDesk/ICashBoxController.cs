namespace Domain.CashDesk;

public interface ICashBoxController
{
    event EventHandler<CashboxAction> ActionTriggered;

    event EventHandler<string>? ListeningFailed;

     void StartListeningToCashbox();
     
}
 
 