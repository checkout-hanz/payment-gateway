namespace Payment.Gateway.Services
{
    public interface ITransactionService
    {
        Task MakeTransaction(Models.CreateTransaction transaction);
    }
}
