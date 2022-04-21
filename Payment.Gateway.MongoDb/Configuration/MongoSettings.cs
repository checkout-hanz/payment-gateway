namespace Payment.Gateway.MongoDb.Configuration
{
    public class MongoSettings : IMongoSettings
    {
        public MongoSettings(string serviceName, string environment, string connectionString)
        {
            ConnectionString = $"{connectionString}&appName={serviceName}-{environment}";
            Database = $"{serviceName}-{environment}";
        }

        public string ConnectionString { get; }
        public string Database { get; }
    }
}

