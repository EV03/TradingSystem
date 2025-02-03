using Domain.StoreSystem.models;
using Shared.Contracts.Dtos;

namespace Store.Application.service;

public interface IStockService
{
    Task<List<StockItem>> GetStockReportAsync();
    Task UpdateStockFromSaleAsync(TransactionDto saleItems);
    Task UpdateStockFromOrderAsync(OrderSupplier orderSupplier, bool isIncoming);
}