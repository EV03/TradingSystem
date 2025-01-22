﻿namespace Domain.CashDesk;

public class  SaleItem
{
    public string Barcode { get; private set; }
    public string Name { get; private set; }
    public long Price { get; private set; }
    public long Quantity { get; set; }

    public SaleItem(string barcode, string name, long price, long quantity)
    {
        Barcode = barcode;
        Name = name;
        Price = price;
        Quantity = quantity;
    }
}