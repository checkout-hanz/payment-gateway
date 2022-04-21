namespace Payment.Gateway.MongoDb.Repositories
{
    public interface ITransactionRepository
    {
        Task InsertTransaction(Models.Transaction transaction);
    }
}

