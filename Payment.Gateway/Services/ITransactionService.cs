using Payment.Gateway.Models;

namespace Payment.Gateway.Services
{
    public interface ITransactionService
    {
        Task<TransactionResponse> MakeTransaction(Models.CreateTransaction createTransaction);
    }
}
