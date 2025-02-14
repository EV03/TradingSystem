using AutoMapper;
using Domain.StoreSystem.models;
using Shared.Contracts.Events;
using Store.Application.service;

namespace Store.Application.EventHandlers;

public class InventorySyncReqEventHandler : IEventHandler<InventorySyncReqEvent>
{
    private readonly IEventBus _eventBus;
    private readonly string _storeId;
    private readonly string _enterpriseId;
    private readonly IStockService _stockService;
    private readonly IMapper _mapper;
    
    public InventorySyncReqEventHandler(IEventBus eventBus, IStockService stockService)
    {
        _eventBus = eventBus;
        _storeId = Environment.GetEnvironmentVariable("STORE_ID") ?? throw new Exception("STORE_ID is not set");
        _enterpriseId = Environment.GetEnvironmentVariable("ENTERPRISE_ID") ?? throw new Exception("ENTERPRISE_ID is not set");
        _stockService = stockService;
        
    }
    public async  Task HandleAsync(InventorySyncReqEvent @event)
    {
        
        if(@event.ExcludedStoreIds.Contains(long.Parse(_storeId)))
        {
            return;  // store is excluded
        }
        // better to filter here for which products we need the stockreport 
        var stock = await _stockService.GetStockReportAsync();
        
        var stockItems = stock.Where(x => @event.ProductIds.Contains(x.CachedProductId)).ToList();
        var productsStock = _mapper.Map<List<InventoryResponseProductsStock>>(stockItems);
        
        foreach (var productStock in productsStock)
        {
            productStock.FromStoreId = long.Parse(_storeId);
        }
        
        var inventorySyncResEvent = new InventorySyncResEvent
        {
            ProductsStock = productsStock,
            EnterpriseId = long.Parse(_enterpriseId),
            ToStoreId = @event.ToStoreId,
            ExcludedStoreIds = @event.ExcludedStoreIds
        };
        
        await _eventBus.PublishAsync(inventorySyncResEvent);
    }
}