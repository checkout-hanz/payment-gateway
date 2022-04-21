using System.Text.Json;
using Payment.Gateway.Messaging.RabbitMq;

namespace Payment.Gateway.Messaging.Publisher
{
    public class Publisher<T> : IPublisher<T> where T : IEvent
    {
        private readonly IMessageBusClient _messageBusClient;
        
        public Publisher(IMessageBusClient messageBusClient)
        {
            _messageBusClient = messageBusClient;
        }

        public async Task PublishAsync(T eventToPublish)
        {
            await Task.Run(() =>
            {
                var message = JsonSerializer.Serialize(eventToPublish);
                _messageBusClient.Publish(message);
            });
        }
    }
}
