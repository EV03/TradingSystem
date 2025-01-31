using Domain.StoreSystem.repository;
using Shared.Contracts.Dtos;
using Store.Application.service;

namespace Store.Application;

public class StockService : IStockService
{
    private readonly IStockItemRepository _stockItemRepository;
    
    public StockService(IStockItemRepository stockItemRepository)
    {
        _stockItemRepository = stockItemRepository;
    }
    public Task<List<StockDto>> GetStockReportAsync(Guid storeId)
    {
     throw new NotImplementedException();
    }

    public Task UpdateStockFromSaleAsync(List<OrderItemDto> orderItems)
    {
        throw new NotImplementedException();
    }

    public Task UpdateStockFromOrderAsync(Guid orderSupplierIds)
    {
        throw new NotImplementedException();
    }
}