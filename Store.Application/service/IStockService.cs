using Shared.Contracts.Dtos;

namespace Store.Application.service;

public interface IStockService
{
    Task<List<StockDto>>  GetStockReportAsync(Guid storeId);
    Task UpdateStockFromSaleAsync(List<OrderItemDto> orderItems);
    Task UpdateStockFromOrderAsync(Guid orderSupplierIds);
}