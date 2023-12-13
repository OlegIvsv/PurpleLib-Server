using Bogus;
using CatalogService.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Data;

public class DataSeeder : IDisposable
{
    private readonly CatalogDbContext _context;
    private readonly IServiceScope _scope;

    public DataSeeder(WebApplication app)
    {
        _scope = app.Services.CreateScope()!;
        _context = _scope.ServiceProvider.GetService<CatalogDbContext>()!;
    }

    public DataSeeder ApplyMigrations()
    {
        _context.Database.Migrate();
        return this;
    }

    public DataSeeder InsertCatalogItems()
    {
        if (_context.CatalogItems.Any())
            return this;

        _context.CatalogItems.AddRange(GenerateCatalogItems(45));
        _context.SaveChanges();

        return this;
    }

    private List<CatalogItem> GenerateCatalogItems(int n)
    {
        var floraPictureFaker = new Faker<FloraPicture>()
            .RuleFor(fp => fp.FloraId, f => f.Random.Guid())
            .RuleFor(fp => fp.Url, f => f.Image.PicsumUrl());

        var floraFaker = new Faker<Flora>()
            .RuleFor(f => f.Name, f => f.Commerce.ProductName())
            .RuleFor(f => f.Color, f => f.Commerce.Color())
            .RuleFor(f => f.LifeTime, f => f.Date.Recent(365).ToUniversalTime())
            .RuleFor(f => f.Pictures, f => floraPictureFaker.GenerateBetween(2, 4));

        var catalogItemFaker = new Faker<CatalogItem>()
            .RuleFor(c => c.Title, f => f.Commerce.ProductName())
            .RuleFor(c => c.OriginalPrice, f => f.Random.Number(2, 50) * 10)
            .RuleFor(c => c.SoldAmount, f => f.Random.Number(0, 50))
            .RuleFor(c => c.Seller, f => f.Internet.UserName())
            .RuleFor(c => c.Status, f => f.PickRandom<ItemStatus>())
            .RuleFor(c => c.OfferEndsAt, f => f.Date.Future(7).ToUniversalTime())
            .RuleFor(c => c.CreatedAt, f => f.Date.Past(1).ToUniversalTime())
            .RuleFor(c => c.UpdatedAt, f => f.Date.Past(1).ToUniversalTime())
            .RuleFor(c => c.Flora, f => floraFaker.Generate());

        return catalogItemFaker.Generate(n);
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}