using Payment.Gateway.MongoDb.Models;

namespace Payment.Gateway.Models
{
    public class TransactionResponse
    {
        public Guid TransactionId { get; set; }
        public PaymentTransactionStatus Status { get; set; }
    }
}
