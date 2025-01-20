
using CashDesk.Integration.Bank;
using Domain.CashDesk;

namespace CashDeskHardwareControllers.CardReaderService;

public class SilaCardReaderAdapter : ICardReaderController
{
    private readonly ICardReaderService _cardReaderService;
    
    public SilaCardReaderAdapter(ICardReaderService cardReaderService)
    
    {
        _cardReaderService = cardReaderService;
    }
    public async Task<ICardReaderResult> WaitForCardReadAsync(long amount, byte[] challenge)
    {
            var authorizationCommand = _cardReaderService.Authorize(amount, new MemoryStream(challenge));

            var authorizationData = await authorizationCommand.Response;

            var cardAuthorization = new CardAuthorization(
                authorizationData.Account,
                authorizationData.AuthorizationToken,
                (int)authorizationData.Amount
            );

            return cardAuthorization;
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