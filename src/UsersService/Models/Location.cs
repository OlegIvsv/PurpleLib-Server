using MongoDB.Driver.GeoJsonObjectModel;

namespace UsersService.Models;

public class Location
{
    public string PlaceId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public GeoJsonPoint<GeoJson2DCoordinates> LocationCoordinates { get; set; } = null!;
}