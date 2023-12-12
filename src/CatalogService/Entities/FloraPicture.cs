namespace CatalogService.Entities;

public class FloraPicture
{
    public Guid Id { get; set; }
    public Guid FloraId { get; set; }
    public string Url { get; set; } = null!;
    public Flora Flora { get; set; } = null!;
}