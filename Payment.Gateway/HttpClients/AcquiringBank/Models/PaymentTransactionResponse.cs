using Payment.Gateway.Models;
using Payment.Gateway.MongoDb.Models;

namespace Payment.Gateway.HttpClients.AcquiringBank.Models
{
    public class PaymentTransactionResponse
    {
        public string TransactionId { get; set; }

        public PaymentTransactionStatus Status { get; set; }
    }
}
