using Domain.CashDesk;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CashDesk.Application;

public class SaleService : ISaleService
{
    private readonly IStoreCommunication _storeCommunication;
    private Sale? _sale; // aktueller verkauf
    
    public SaleService(IStoreCommunication storeCommunication)
    {
        _storeCommunication = storeCommunication;
    }
    public void StartSaleAsync()
    {
        if (_sale != null)
        {
           throw new InvalidOperationException("A sale is already in progress.");
        }
        _sale = new Sale();
    }

    public async Task<OperationResult> AddProductToSale(string barcode)
    {
        try
        {
            var product = await _storeCommunication.GetProduct(barcode);

            _sale.AddItem(new SaleItem(product.Barcode, product.Name, product.Price, product.Quantity));
            return OperationResult.Success();
        }
        catch (ProductNotFoundException ex)
        {
            return OperationResult.Canceled();
        }
        catch (Exception e)
        {   Console.WriteLine("Error during adding product to sale: " + e.Message);
            return OperationResult.Failure(e.Message);
        }
    }

    public int GetSaleTotalAsync()
    {
        return _sale.Total;
    }

    public Task<OperationResult> FinishSaleAsync(Transaction transaction)
    {
        throw new NotImplementedException();
    }
    
}