using Domain.CashDesk;
using Shared.Contracts.Interfaces;

namespace CashDesk.Application;

public class PaymentService : IPaymentService
{
    IBankService _bankService;
    ISaleService _saleService;
    ICardReaderController _cardReaderController;

    public PaymentService(IBankService bankService, ISaleService saleService,
        ICardReaderController cardReaderController)
    {
        _bankService = bankService;
        _saleService = saleService;
        _cardReaderController = cardReaderController;
    }

    // cash payment is handled by the cashier so we just let it return true at this point
    // may be extended in the future
    public async Task PayCashAsync(long amount)
    {

    }

    public async Task PayCardAsync(long amount)
    {

        // start with creation of a bank context
        var context = await _bankService.CreateTransactionContextAsync(amount);

        // prepare the cardreader to read the card 
        var cardResult = await _cardReaderController.WaitForCardReadAsync(amount, context.Challenge);

        // after card is read, authorize the payment with the bank
        await _bankService.AuthorizePaymentAsync(
            context.Id, cardResult.Account, cardResult.Token);

    }
}
       