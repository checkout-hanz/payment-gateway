namespace Payment.Gateway.Messaging.Subscription.MerchantCreated
{
    public class MerchantCreatedEvent : IMessage
    {
        public MerchantCreatedEvent()
        {
            EventName = nameof(MerchantCreatedEvent);
        }

        public string EventName { get; set; }

        public Guid MerchantId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
