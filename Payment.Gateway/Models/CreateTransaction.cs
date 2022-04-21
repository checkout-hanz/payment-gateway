namespace Payment.Gateway.Models
{
    public class CreateTransaction
    {
        public Guid MerchantId { get; set; }

        public decimal Amount { get; set; }

        public string CardNumber { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public string Currency { get; set; }

        public string CVV { get; set; }

    }
}