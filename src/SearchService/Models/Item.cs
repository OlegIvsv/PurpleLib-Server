namespace SearchService.Models;

public class Item
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Name { get; set; }
    public string? Color { get; set; }
    public int OriginalPrice { get; set; }
    public Guid SellerId { get; set; }
    public int TotalAmount { get; set; }
    public int SoldAmount { get; set; }
    public string Status { get; set; }
    public List<string> Pictures { get; set; }
    public DateTime? LifeTime { get; set; }
    public DateTime OfferEndsAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}