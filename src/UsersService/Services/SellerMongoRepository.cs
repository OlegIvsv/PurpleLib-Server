using MongoDB.Driver;
using UsersService.Models;
using MongoDB.Driver.Linq;

namespace UsersService.Services;

public class SellerMongoRepository : ISellerRepository
{
    private readonly IMongoCollection<Seller> _sellerCollection;
    private readonly IMongoDatabase _database;

    public SellerMongoRepository(string connectionString)
    {
        var mongoClient = new MongoClient(connectionString);
        _database = mongoClient.GetDatabase("user-db");
        _sellerCollection = _database.GetCollection<Seller>("sellers");
    }

    public async Task CreateAsync(Seller seller)
    {
        await _sellerCollection.InsertOneAsync(seller);
    }

    public async Task<Seller?> GetByIdAsync(Guid id)
    {
        return await _sellerCollection.AsQueryable().FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Seller?> EditAsync(Seller seller)
    {
        var filter = Builders<Seller>.Filter.Eq(s => s.Id, seller.Id);
        var options = new ReplaceOptions { IsUpsert = true };
        
        await _sellerCollection.ReplaceOneAsync(filter, seller, options); 
        
        return await _sellerCollection.Find(filter).FirstOrDefaultAsync();
    }
}