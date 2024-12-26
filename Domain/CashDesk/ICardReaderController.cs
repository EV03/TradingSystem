namespace Domain.CashDesk;

public interface ICardReaderController
{
    Task<CardAuthorization> WaitForCardReadAsync();
   void Confirm();

   void Abort(string message);

}