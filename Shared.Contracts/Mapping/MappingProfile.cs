using AutoMapper;
using Domain.Enterprise.ValueObjects;
using Domain.StoreSystem;
using Domain.StoreSystem.models;
using Domain.StoreSystem.ValueObjects;
using Shared.Contracts.Dtos;

namespace Shared.Contracts.Mapping;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<OrderProductDto, OrderProduct>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

        CreateMap<CachedProduct, ProductDto>()
            .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.Barcode))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.CurrentPrice))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId));
        
        CreateMap<StockItem, StockDto>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.CachedProductId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.AvailableQuantity, opt => opt.MapFrom(src => src.AvailableQuantity))
            .ForMember(dest => dest.IncomingQuantity, opt => opt.MapFrom(src => src.IncomingQuantity));
        
        CreateMap<SupplierDeliveryTime, SupplierDeliveryTimeDto>()
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.SupplierName))
            .ForMember(dest => dest.AverageDeliveryTime, opt => opt.MapFrom(src => src.AverageDeliveryTime));
    }
}