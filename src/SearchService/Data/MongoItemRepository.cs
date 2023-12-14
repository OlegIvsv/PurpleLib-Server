using MongoDB.Driver;
using SearchService.Contracts;
using SearchService.Models;

namespace SearchService.Data;

public class MongoItemRepository : IItemRepository
{
    private readonly IMongoCollection<Item> _itemCollection;
    private readonly IMongoDatabase _database;

    public MongoItemRepository(string connectionString)
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

    public async Task<PaginationResult<Item>> RunSearch(string query, int page, int pageSize)
    {
        int preventedPage = Math.Max(0, page);
        int preventedPageSize = Math.Max(1, pageSize);

        var filter = Builders<Item>.Filter.Text(query, new TextSearchOptions() { CaseSensitive = false });
        var sort = Builders<Item>.Sort.MetaTextScore("score");

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
}