namespace CatalogService.Contracts;

public class CatalogItemResponse
{
    public Guid Id { get; set; }
    public string Title { get; init; }
    public int OriginalPrice { get; init; }
    public int SoldAmount { get; init; }
    public string Seller { get; init; }
    public Guid FloraId { get; init; }
    public string Status { get; init; }
    public DateTime OfferEndsAt { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string Name { get; init; }
    public string? Color { get; init; }
    public DateTime? LifeTime { get; init; }
    public List<string> Pictures { get; init; }
}