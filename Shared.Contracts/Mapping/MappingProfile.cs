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
            .ForMember(dest => dest.CachedProductId, opt => opt.MapFrom(src => src.CachedProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
        
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OrderSupplier, opt => opt.MapFrom(src => src.OrderSupplier));
        
        CreateMap<OrderSupplier, OrderSupplierDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.DeliveryDate, opt => opt.MapFrom(src => src.DeliveryDate))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.OrderSupplierProducts, opt => opt.MapFrom(src => src.OrderSupplierProducts));
        
        CreateMap<OrderSupplierCachedProduct, OrderSupplierCachedProductDto>()
            .ForMember(dest => dest.CachedProductId, opt => opt.MapFrom(src => src.CachedProductId))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
        
        CreateMap<CachedProduct, ProductDto>()
            .ForMember(dest => dest.Barcode, opt => opt.MapFrom(src => src.Barcode))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src =>(int) (src.CurrentPrice * 100 )))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        
        
        CreateMap<StockItem, StockDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CachedProductId, opt => opt.MapFrom(src => src.CachedProductId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.AvailableQuantity, opt => opt.MapFrom(src => src.AvailableQuantity))
            .ForMember(dest => dest.IncomingQuantity, opt => opt.MapFrom(src => src.IncomingQuantity))
            .ForMember(dest => dest.OutGoingQuantity, opt => opt.MapFrom(src => src.OutGoingQuantity));
        
        CreateMap<SupplierDeliveryTime, SupplierDeliveryTimeDto>()
            .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.SupplierName))
            .ForMember(dest => dest.AverageDeliveryTime, opt => opt.MapFrom(src => src.AverageDeliveryTime));
    }
}