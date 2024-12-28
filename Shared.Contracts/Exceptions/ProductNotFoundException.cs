namespace Shared.Contracts.Exceptions;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(string barcode)
        : base($"Product with barcode {barcode} not found.")
    {
    } 
}