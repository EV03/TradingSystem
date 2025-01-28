using Shared.Contracts.Dtos;

namespace Store.Application.service;

public interface IStockService
{
    
    Task<List<StockDto>>  GetStockReportAsync(Guid storeId);
    
    Task UpdateStockAsync(TransactionDto transactionDto);
    
    Task UpdateStockAsync(List<OrderItemDto> orderItems);
    
    Task UpdateStockItemPriceAsync(Guid stockItemId, decimal newPrice);
}