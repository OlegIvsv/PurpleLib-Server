using UsersService.Models;

namespace UsersService.Contracts;

public class CreateSellerRequest
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<string> Pictures { get; set; } = new();
    public LocationViewModel? Location { get; set; } 
}