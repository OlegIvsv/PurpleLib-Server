using System.Linq.Expressions;
using MongoDB.Driver;
using SearchService.Contracts;
using SearchService.Models;

namespace SearchService.Data;

public class ItemService : IItemService
{
    private readonly IMongoCollection<Item> _itemCollection;
    private readonly IMongoDatabase _database;

    public ItemService(string connectionString)
    {
        var mongoClient = new MongoClient(connectionString);
        _database = mongoClient.GetDatabase("search-db");
        _itemCollection = _database.GetCollection<Item>("items");

        EnsureTextIndexesCreated();
    }

    private void EnsureTextIndexesCreated()
    {
        var titleIndex = Builders<Item>.IndexKeys.Text(item => item.Title);
        var nameIndex = Builders<Item>.IndexKeys.Text(item => item.Name);
        var colorIndex = Builders<Item>.IndexKeys.Text(item => item.Color);

        var indexKeysDefinition = Builders<Item>.IndexKeys.Combine(titleIndex, nameIndex, colorIndex);
        var createIndexOptions = new CreateIndexOptions { DefaultLanguage = "english" };
        var indexModel = new CreateIndexModel<Item>(indexKeysDefinition, createIndexOptions);

        _itemCollection.Indexes.CreateOne(indexModel);
    }

    public async Task Insert(Item item)
    {
        await _itemCollection.InsertOneAsync(item);
    }

    public async Task Update(Item item)
    {
        var filter = Builders<Item>.Filter.Eq(i => i.Id, item.Id);
        await _itemCollection.ReplaceOneAsync(filter, item);
    }

    public async Task Delete(Guid id)
    {
        var filter = Builders<Item>.Filter.Eq(i => i.Id, id);
        await _itemCollection.DeleteOneAsync(filter);
    }

    public async Task<PaginationResult<Item>> RunSearch(
        string? query,
        int page,
        int pageSize,
        string sortOrder,
        string sortProperty)
    {
        int preventedPage = Math.Max(1, page);
        int preventedPageSize = Math.Max(1, pageSize);

        var filter = ItemFilter(query);
        var sort = ItemSorting(sortOrder, sortProperty);

        var items = await _itemCollection
            .Find(filter)
            .Sort(sort)
            .Skip((preventedPage - 1) * preventedPageSize)
            .Limit(preventedPageSize)
            .ToListAsync();
        var total = await _itemCollection.CountDocumentsAsync(filter);

        return new()
        {
            Total = total,
            Count = items.Count,
            Items = items
        };
    }

    private SortDefinition<Item> ItemSorting(string sortOrder, string sortProperty)
    {
        var sort = Builders<Item>.Sort;
        Expression<Func<Item, object>>? propertySelector;

        switch (sortProperty)
        {
            case "title":
                propertySelector = x => x.Title;
                break;
            case "name":
                propertySelector = x => x.Name;
                break;
            case "price":
                propertySelector = x => x.OriginalPrice;
                break;
            case "date":
                propertySelector = x => x.CreatedAt;
                break;
            default:
                propertySelector = x => x.CreatedAt;
                break;
        }

        sort.MetaTextScore("score");

        if (sortOrder == "asc")
            return sort.Ascending(propertySelector);
        else
            return sort.Descending(propertySelector);
    }

    private FilterDefinition<Item> ItemFilter(string? query)
    {
        var filter = Builders<Item>.Filter;
        if (string.IsNullOrWhiteSpace(query))
            return filter.Empty;
        return filter.Text(query, new TextSearchOptions() { CaseSensitive = false });
    }
}