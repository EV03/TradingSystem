namespace Store.Application.service;

public interface IProductService
{
    Task UpdatePrice (Guid stockItemId, decimal newPrice);
}