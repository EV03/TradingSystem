﻿using System.Text.Json.Serialization;

namespace Shared.Contracts.Events;

public class  InventorySyncResEvent : IStoreEvent 
{
    public long EnterpriseId { get; set; }
    public long ToStoreId { get; set; }
    public List<long> ExcludedStoreIds { get; set; } = new List<long>();
    public  List<InventoryResponseProductsStock> ProductsStock{ get; set; } = new List<InventoryResponseProductsStock>();
}
public class InventoryResponseProductsStock
{ 
    public long ProductId { get; set; }
    public long minStock { get; set; }
    public  long FromStoreId { get; set; }
    public int Quantity { get; set; }
}