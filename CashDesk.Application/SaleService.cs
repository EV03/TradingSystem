using Domain.CashDesk;
using Shared.Contracts.Dtos;
using Shared.Contracts.Exceptions;
using Shared.Contracts.Interfaces;

namespace CashDesk.Application;

public class SaleService : ISaleService  
{
    private readonly IStoreCommunication _storeCommunication;
    private Sale _sale; // aktueller verkauf
    
    public SaleService(IStoreCommunication storeCommunication, Sale sale)
    {
        _storeCommunication = storeCommunication;
        _sale = null!;
    }

    public Sale Sale { get => _sale; }

    public void StartSale()
    {
        _sale = new Sale();
    }

    public async Task<SaleItem> AddProductToSale(string barcode)
    {
        if (_sale == null)  throw new InvalidOperationException("No sale in progress.");
        
        var product = await _storeCommunication.GetProduct(barcode);
        
        _sale.AddItem(new SaleItem(product.Barcode, product.Name, product.Price));
        
        return _sale.Items.First((item) => item.Barcode == barcode);

    }

    public long GetSaleTotal()
        
    {
         return _sale.Total;
       
    }

    public async Task FinishSaleAsync()
    {
        if (_sale.IsEmpty()) throw new InvalidOperationException("No sale in progress.");
        
        var transactionDto = new TransactionDto
        {
            Items = _sale.Items.ToDictionary(i => i.Barcode, i => i.Quantity)
        };
        // todo : safe the sale somewhere
        try
        {
            await _storeCommunication.UpdateInventory(transactionDto);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error trying to update the inventory " + e.Message);
            throw;
        }
    }

    // todo: implement barcode validation
    public bool IsValidBarcode(string barcode)
    {
        return true;
    }
}