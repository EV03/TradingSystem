﻿namespace Domain.CashDesk;

public class  SaleItem
{
    public string Barcode { get; private set; }
    public string Name { get; private set; }
    public int Price { get; private set; }
    public int Quantity { get; set; }

    public SaleItem(string barcode, string name, int price)
    {
        Barcode = barcode;
        Name = name;
        Price = price;
        Quantity = 1;
    }
}