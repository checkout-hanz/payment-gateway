using Payment.Gateway.MongoDb.Configuration;
using MongoDB.Driver;

namespace Payment.Gateway.MongoDb.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly IMongoContext _mongoContext;

    public TransactionRepository(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    public async Task InsertTransaction(Models.Transaction transaction)
    {
        await _mongoContext.Transactions.InsertOneAsync(transaction);
    }
}