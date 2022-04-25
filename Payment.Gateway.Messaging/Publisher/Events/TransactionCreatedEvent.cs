using Payment.Gateway.MongoDb.Models;

namespace Payment.Gateway.Messaging.Publisher.Events
{
    public class TransactionCreatedEvent : IEvent
    {
        public TransactionCreatedEvent()
        {
            EventName = nameof(TransactionCreatedEvent);
        }

        public string EventName { get; set; }
        public Guid TransactionId {get;set;}
        public Guid MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string CardNumber { get; set; }
        public string Currency { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public PaymentTransactionStatus Status {get; set; }
    }
}
