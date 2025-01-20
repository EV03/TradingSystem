using Domain.CashDesk;

namespace CashDesk.Application;

public class ExpressModeService : IExpressModeService
{
    public bool IsExpressMode()
    {
        return true;
    }
}