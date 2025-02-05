﻿using AutoMapper;
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
        
        CreateMap<CachedProduct, StockDto>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.AvailableQuantity, opt => opt.MapFrom(src => src.StockItem.AvailableQuantity))
            .ForMember(dest => dest.IncomingQuantity, opt => opt.MapFrom(src => src.StockItem.IncomingQuantity));
    }
}