using Domain.CashDesk;
using Shared.Contracts.Dtos;
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
    public void StartSale()
    {
        if (_sale != null)
        {
           throw new InvalidOperationException("A sale is already in progress.");
        }
        _sale = new Sale();
    }

    public async Task<OperationResult> AddProductToSale(string barcode)
    {
        if (_sale == null)
        {
            throw new InvalidOperationException("No sale in progress.");
        }
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

    public int GetSaleTotal()
    {
        if (_sale != null) return _sale.Total;
        throw new InvalidOperationException("No sale in progress.");
    }

    public async Task<OperationResult> FinishSaleAsync()
    {
        if (_sale == null || _sale.IsEmpty())
        {
            return OperationResult.Canceled();
        }
        
        var transactionDto = new TransactionDto
        {
            Items = _sale.Items.ToDictionary(i => i.Barcode, i => i.Quantity)
        };
        _sale = null;
        try
        {
             await _storeCommunication.UpdateInventory(transactionDto);
           
            return OperationResult.Success();
        }
        catch (Exception e)
        {
            Console.WriteLine("Error trying to update the inventory " + e.Message);
            return OperationResult.Failure(e.Message);
        }
    }

    public bool isValidBarcode(string barcode)
    {
        return true;
    }
}