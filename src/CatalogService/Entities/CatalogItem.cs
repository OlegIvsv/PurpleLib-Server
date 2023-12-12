namespace CatalogService.Entities;

public class CatalogItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public int OriginalPrice { get; set; }
    public int SoldAmount { get; set; }
    public string Seller { get; set; } = null!;
    public Guid FloraId { get; set; }
    public ItemStatus Status { get; set; }
    public DateTime OfferEndsAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public Flora Flora { get; set; } = null!;
}