namespace Domain.CashDesk;

public interface ICardReaderController
{   Task<OperationResult<CardAuthorization>> WaitForCardReadAsync(int amount, byte[] challenge);
   void Confirm(string message);

   void Abort(string message);

}