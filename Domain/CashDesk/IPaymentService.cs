namespace Domain.CashDesk;

public interface IPaymentService
{
    Task<bool> PayByCash(int amount);
    
    Task<bool> PayByCard(int amount);
}