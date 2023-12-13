using AutoMapper;
using CatalogService.Contracts;
using CatalogService.Entities;

namespace CatalogService.RequestHelpers.Mapping;

public class CatalogItemProfile : Profile
{
    public CatalogItemProfile()
    {
        CreateMap<CatalogItem, CatalogItemResponse>()
            .IncludeMembers(x => x.Flora);

        CreateMap<Flora, CatalogItemResponse>()
            .ForMember(cir => cir.Pictures,
                opt => opt.MapFrom(f => f.Pictures.Select(p => p.Url).ToList()));


        CreateMap<CatalogItemCreateRequest, CatalogItem>()
            .ForMember(ci => ci.Flora,
                opt => opt.MapFrom(ci => ci));

        CreateMap<CatalogItemCreateRequest, Flora>()
            .ForMember(f => f.Pictures,
                opt => opt.MapFrom(ci => ci.Pictures));


        CreateMap<CatalogItemUpdateRequest, CatalogItem>()
            .ForMember(ci => ci.Flora,
                opt => opt.MapFrom(ci => ci));

        CreateMap<CatalogItemUpdateRequest, Flora>()
            .ForMember(f => f.Pictures,
                opt => opt.MapFrom(ci => ci.Pictures));


        CreateMap<string, FloraPicture>()
            .ConvertUsing(s => new FloraPicture() { Url = s });
    }
}