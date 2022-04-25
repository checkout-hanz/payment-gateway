using Payment.Gateway.HttpClients.AcquiringBank.Models;

namespace Payment.Gateway.HttpClients.AcquiringBank
{
    public interface IAcquiringBankClient
    {
        Task<PaymentTransactionResponse> MakePayment(PaymentTransactionRequest request);
    }
}
