using Payment.Gateway.Messaging.Publisher;
using Payment.Gateway.Messaging.Publisher.Events;
using Payment.Gateway.Messaging.RabbitMq;
using Payment.Gateway.Messaging.Subscription;
using Payment.Gateway.Messaging.Subscription.MerchantCreated;
using MerchantCreatedEvent = Payment.Gateway.Messaging.Subscription.MerchantCreated.MerchantCreatedEvent;

namespace Payment.Gateway.Configuration
{
    public static class MessagingConfiguration
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSingleton<IPublisher<MerchantCreatedEvent>, Publisher<MerchantCreatedEvent>>();

            var host = configuration["RabbitMQHost"];
            var port = int.Parse(configuration["RabbitMQPort"]);
            services.AddSingleton<IRabbitMQConfig>(_ => new RabbitMQConfig(host, port));
            services.AddSingleton<IEventProcessor, EventProcessor>();
            services.AddSingleton<IMessageBusClient, MessageBusClient>();//(_ => new MessageBusClient(host, port));
            services.AddHostedService<MessageBusSubscriber>();
            services.AddSingleton<ISubscriptionHandler<MerchantCreatedEvent>, MerchantCreatedHandler>();
            return services;
        }
    }
}
