
using Shared.Contracts.Dtos;

namespace Shared.Contracts.Interfaces;

public interface IStoreCommunication
{
    Task<ProductDto> GetProduct(string barcode);
    
    Task UpdateInventory (TransactionDto transaction);
}