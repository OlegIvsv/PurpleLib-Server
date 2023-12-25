namespace UsersService.Models;

public class LocationViewModel
{
    public string PlaceId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}