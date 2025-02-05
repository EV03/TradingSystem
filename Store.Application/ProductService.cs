using Domain.StoreSystem.models;
using Domain.StoreSystem.repository;
using Store.Application.service;

namespace Store.Application;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<CachedProduct> ChangePrice(Guid productId, float newPrice)
    {
        var product = await _productRepository.GetByProductIdAsync(productId);
        if (product == null)
        {
            throw new ArgumentException("Product not found");
        }
        product.CurrentPrice = newPrice; 
        await _productRepository.Update(product);
        return product;
    }

    public async Task<List<CachedProduct>> ShowAllProductsAsync()
    {
        var products = await _productRepository.GetProductsAsync();
        if (products == null)
        {
            throw new ArgumentException("No products found for the given store");
        }
       return products.ToList()!;
    }

    public async  Task<CachedProduct> ShowProductDetails(string barcode)
    {
        var product = await _productRepository.GetByBarcodeAsync(barcode);
                
        if (product == null)
        {
            throw new ArgumentException("Product not found");
        }
        return product;
    }
    
} 