using AutoMapper;
using MongoDB.Driver.GeoJsonObjectModel;
using UsersService.Contracts;
using UsersService.Models;

namespace UsersService.RequestHelpers.Mapping;

public class SellerProfile : Profile
{
    public SellerProfile()
    {
        CreateMap<CreateSellerRequest, Seller>()
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => MapLocationViewModelToLocation(src.Location)));

        CreateMap<EditSellerRequest, Seller>()
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => MapLocationViewModelToLocation(src.Location)));

        CreateMap<Seller, GetSellerResponse>()
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => MapLocationToLocationViewModel(src.Location)));
    }

    private Location? MapLocationViewModelToLocation(LocationViewModel? locationViewModel)
    {
        return locationViewModel is null
            ? null
            : new()
            {
                PlaceId = locationViewModel.PlaceId,
                Name = locationViewModel.Name,
                LocationCoordinates = new GeoJsonPoint<GeoJson2DCoordinates>(
                    new GeoJson2DCoordinates(locationViewModel.Longitude, locationViewModel.Latitude))
            };
    }

    private LocationViewModel? MapLocationToLocationViewModel(Location? location)
    {
        return location is null
            ? null
            : new()
            {
                PlaceId = location.PlaceId,
                Name = location.Name,
                Longitude = location.LocationCoordinates.Coordinates.X,
                Latitude = location.LocationCoordinates.Coordinates.Y
            };
    }
}