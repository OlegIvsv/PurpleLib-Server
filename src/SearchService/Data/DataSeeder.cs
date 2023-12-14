using System.Text.Json;
using MongoDB.Driver;
using SearchService.Models;

namespace SearchService.Data;

public class DataSeeder
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<Item> _itemCollection;
    private readonly MongoClient _mongoClient;

    public DataSeeder(WebApplication app)
    {
        var connectionString = app.Configuration.GetConnectionString("DefaultMongoConnection");
        _mongoClient = new MongoClient(connectionString);
        _database = _mongoClient.GetDatabase("search-db");
        _itemCollection = _database.GetCollection<Item>("items");
    }

    public DataSeeder SeedWithItems()
    {
        if (_itemCollection.CountDocuments(_ => true) > 0)
            return this;

        string jsonFilePath = "./Data/seed-data.json";
        string jsonContent = File.ReadAllText(jsonFilePath);
        List<Item> items = JsonSerializer.Deserialize<List<Item>>(jsonContent,
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            })!;
        _itemCollection.InsertMany(items);
        return this;
    }
}