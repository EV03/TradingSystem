using Domain.CashDesk;

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
    public async Task<bool> PayCashAsync(int amount)
    {
        // handle pseudo await for demonstration purposes
        await Task.Delay(1000);
        return true;
    }

    public async Task<bool> PayCardAsync(int amount)
    {
        try
        {
            // start with creation of a bank context
            var contextResult = await _bankService.CreateTransactionContextAsync(amount);
            if (!contextResult.IsSuccess || contextResult.Result == null)
            {
                Console.WriteLine("Failed to create transaction context.");
                return false;
            }
            // get the context
            var context = contextResult.Result;

            // prepare the cardreader to read the card 
            var cardAuthorizationResult = await _cardReaderController.WaitForCardReadAsync(amount, context.Challenge);
            if (!cardAuthorizationResult.IsSuccess || cardAuthorizationResult.Result == null)
            {
                Console.WriteLine("Card authorization failed.");
                return false;
            }

            var cardAuthorization = cardAuthorizationResult.Result;

            // after card is read, authorize the payment with the bank
            var authorizationResult = await _bankService.AuthorizePaymentAsync(
                context.Id, cardAuthorization.Account, cardAuthorization.Token);
            if (!authorizationResult.IsSuccess)
            {
                Console.WriteLine("Payment authorization failed.");
                return false;
            }
             // payment was successful
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during payment: {ex.Message}");
            return false;
        }
    }

}
       