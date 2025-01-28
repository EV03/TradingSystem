using AutoMapper;
using Domain.StoreSystem;
using Shared.Contracts.Dtos;

namespace Shared.Contracts.Mapping;

public class MappingProfile : Profile

{
    public  MappingProfile()
    {
        CreateMap<Order,OrderDto>();
    }
}