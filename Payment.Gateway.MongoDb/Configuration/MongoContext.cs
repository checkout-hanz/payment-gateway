using MongoDB.Driver;

namespace Payment.Gateway.MongoDb.Configuration
{
    public class MongoContext : IMongoContext
    {
        private readonly Lazy<IMongoDatabase> _database;

        private readonly Lazy<IMongoCollection<Payment.Gateway.MongoDb.Models.Merchant>> _messageRecords;

        private readonly Lazy<IMongoCollection<Payment.Gateway.MongoDb.Models.Transaction>> _transactions;

        public MongoContext(IMongoClient mongoClient, IMongoSettings mongoSettings)
        {
            Client = mongoClient;
            _database = new Lazy<IMongoDatabase>(() => mongoClient.GetDatabase(mongoSettings.Database));

            Lazy<IMongoCollection<T>> CreateLazyCollection<T>(string collectionName)
            {
                return new Lazy<IMongoCollection<T>>(() => Database.GetCollection<T>(collectionName));
            }

            _messageRecords = CreateLazyCollection<Payment.Gateway.MongoDb.Models.Merchant>(DatabaseConstants.MerchantCollection);

            _transactions = CreateLazyCollection<Payment.Gateway.MongoDb.Models.Transaction>(DatabaseConstants.TransactionCollection);
        }

        public IMongoClient Client { get; }
        public IMongoDatabase Database => _database.Value;
        public IMongoCollection<Payment.Gateway.MongoDb.Models.Merchant> Merchants => _messageRecords.Value;

        public IMongoCollection<Payment.Gateway.MongoDb.Models.Transaction> Transactions => _transactions.Value;
    }
}
