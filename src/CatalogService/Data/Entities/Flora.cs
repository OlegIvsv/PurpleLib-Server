namespace CatalogService.Entities;

public class Flora
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Color { get; set; } 
    public DateTime? LifeTime { get; set; }

    public CatalogItem CatalogItem { get; set; } = null!; 
    public List<FloraPicture> Pictures { get; set; } = new();
}