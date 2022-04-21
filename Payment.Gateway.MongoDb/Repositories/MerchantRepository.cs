using Payment.Gateway.MongoDb.Configuration;
using MongoDB.Driver;

namespace Payment.Gateway.MongoDb.Repositories;

public class MerchantRepository : IMerchantRepository
{
    private readonly IMongoContext _mongoContext;

    public MerchantRepository(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    public async Task InsertMerchant(Models.Merchant merchant)
    {
        await _mongoContext.Merchants.InsertOneAsync(merchant);
    }

    public async Task<Models.Merchant> GetMerchant(Guid id)
    {
        return await _mongoContext.Merchants.Find(m => m.MerchantId == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Models.Merchant>> GetMerchants()
    {
        return await _mongoContext.Merchants.Find(x => true).ToListAsync();
    }
}