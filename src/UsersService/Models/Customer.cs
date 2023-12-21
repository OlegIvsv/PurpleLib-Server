namespace UsersService.Models;

public class Customer
{
    public Guid Id { get; set; }
    public string ProviderUserId { get; set; } = null!;
    public string Name { get; set; }
    public string? Email { get; set; }
}