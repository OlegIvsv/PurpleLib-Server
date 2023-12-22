namespace UsersService.Contracts;

public class EditSellerRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Description { get; set; }
    public List<string> Pictures { get; set; } = new();
}