﻿using Domain.CashDesk;
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

    public async Task AddProductToSale(string barcode)
    {
        if (_sale == null)  throw new InvalidOperationException("No sale in progress.");
        
        var product = await _storeCommunication.GetProduct(barcode);
        
        _sale.AddItem(new SaleItem(product.Barcode, product.Name, product.Price, product.Quantity));
            
    }

    public long GetSaleTotal()
    {
        if (_sale != null) return _sale.Total;
        throw new InvalidOperationException("No sale in progress.");
    }

    public async Task FinishSaleAsync()
    {
        if (_sale == null || _sale.IsEmpty()) throw new InvalidOperationException("No sale in progress.");
        
        
        var transactionDto = new TransactionDto
        {
            Items = _sale.Items.ToDictionary(i => i.Barcode, i => i.Quantity)
        };
        // todo : safe the sale somewhere
        _sale = null;
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