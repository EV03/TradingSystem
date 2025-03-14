﻿namespace Domain.CashDesk;

public interface ISaleService
{
    Sale Sale { get; }
    void StartSale();
    Task<SaleItem> AddProductToSale(string barcode); 
    long GetSaleTotal();
    Task FinishSaleAsync();
    
    bool IsValidBarcode(string barcode);
}
