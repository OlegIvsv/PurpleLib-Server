namespace UsersService.Contracts;

public class CreateSellerRequest
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}