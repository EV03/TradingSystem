
using Domain.CashDesk;

namespace CashDeskHardwareControllers.CardReaderService;

public class SilaCardReaderAdapter : ICardReaderController
{
    private readonly ICardReaderService _cardReaderService;
    
    public SilaCardReaderAdapter(ICardReaderService cardReaderService)
    {
        _cardReaderService = cardReaderService;
    }
    
    public Task<CardAuthorization> WaitForCardReadAsync()
    {
        throw new NotImplementedException();
    }

    public void Confirm()
    {
        throw new NotImplementedException();
    }
    
    public void Abort(string message)
    {
        throw new NotImplementedException();
    }
}