namespace Payment.Gateway.MongoDb.Configuration
{
    public interface IMongoSettings
    {
        string ConnectionString { get; }
        string Database { get; }
    }
}
