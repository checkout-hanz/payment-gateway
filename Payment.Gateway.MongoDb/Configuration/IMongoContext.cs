using MongoDB.Driver;

namespace Payment.Gateway.MongoDb.Configuration
{
    public interface IMongoContext
    {
        IMongoClient Client { get; }
        IMongoDatabase Database { get; }

        IMongoCollection<Payment.Gateway.MongoDb.Models.Merchant> Merchants { get; }

        IMongoCollection<Payment.Gateway.MongoDb.Models.Transaction> Transactions { get; }
    }
}