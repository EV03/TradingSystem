
using Domain.CashDesk;

namespace CashDeskHardwareControllers.CardReaderService;

public class SilaCardReaderAdapter : ICardReaderController
{
    private readonly ICardReaderService _cardReaderService;
    
    public SilaCardReaderAdapter(ICardReaderService cardReaderService)
    {
        _cardReaderService = cardReaderService;
    }
    public async Task<OperationResult<CardAuthorization>> WaitForCardReadAsync(int amount, byte[] challenge)
    {
        try
        {
            var authorizationCommand = _cardReaderService.Authorize(amount, new MemoryStream(challenge));
            
            var authorizationData = await authorizationCommand.Response;

            var cardAuthorization = new CardAuthorization(
                authorizationData.Account,
                authorizationData.AuthorizationToken,
                (int)authorizationData.Amount
            );

            return OperationResult<CardAuthorization>.Success(cardAuthorization);
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Card reading operation was canceled.");
            return OperationResult<CardAuthorization>.Canceled();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during card reading: " + ex.Message);
            return OperationResult<CardAuthorization>.Failure(ex.Message);
        }
    }

    public void Confirm(string message)
    {
        _cardReaderService.Confirm();
    }
    
    public void Abort(string message)
    {
        _cardReaderService.Abort(message);
    }
}