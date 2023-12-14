using AutoMapper;
using Contracts.CatalogItem;
using SearchService.Models;

namespace SearchService.RequestHelpers.Mapping;

public class ItemProfile : Profile
{
    public ItemProfile()
    {
        CreateMap<CatalogItemCreated, Item>();
        CreateMap<CatalogItemUpdated, Item>();
    }
}