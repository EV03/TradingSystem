namespace Domain.CashDesk;

public interface ICashBoxController
{
    event EventHandler<CashDeskAction> ActionTriggered;

    event EventHandler<string>? ListeningFailed;

     void StartListeningToCashbox();
     
     void StopListeningToCashbox();
     
}
 
  