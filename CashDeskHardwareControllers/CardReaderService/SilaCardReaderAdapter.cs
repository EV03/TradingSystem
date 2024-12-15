using Domain.CashDesk;

namespace CashDeskHardwareControllers.CardReaderService;

public class SilaCardReaderAdapter : ICardReaderController
{
    private readonly ICardReaderService _cardReaderService;
    
    public SilaCardReaderAdapter(ICardReaderService cardReader)
    {
        _cardReaderService = cardReader;
    }

    public bool Pay()
    {
        throw new NotImplementedException();
    }
}