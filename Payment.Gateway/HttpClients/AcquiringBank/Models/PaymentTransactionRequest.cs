namespace Payment.Gateway.HttpClients.AcquiringBank.Models
{
    public class PaymentTransactionRequest
    {
        public Guid TransactionId { get; set; }

        public string ProviderId { get; set; }

        public decimal Amount { get; set; }

        public string CardNumber { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public string Currency { get; set; }

        public string CVV { get; set; }
    }
}
