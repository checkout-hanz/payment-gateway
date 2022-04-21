namespace Payment.Gateway.MongoDb.Repositories
{
    public interface IMerchantRepository
    {
        Task InsertMerchant(Models.Merchant message);
        Task<Models.Merchant> GetMerchant(Guid id);
        Task<IEnumerable<Models.Merchant>> GetMerchants();
    }
}

