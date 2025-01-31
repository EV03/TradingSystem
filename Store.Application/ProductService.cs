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
    
    public Task UpdatePrice(Guid stockItemId, decimal newPrice)
    {
        throw new NotImplementedException();
    }
}