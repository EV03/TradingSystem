using Domain.StoreSystem.models;

namespace Store.Application.service;

public interface IProductService
{
    Task<CachedProduct> ChangePrice (Guid cachedProductId, double newPrice);
    Task<List<CachedProduct>> ShowAllProductsAsync();
    Task<CachedProduct> ShowProductDetails(string barcode );
}