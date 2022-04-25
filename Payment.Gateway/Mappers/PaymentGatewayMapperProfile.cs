using AutoMapper;
using Payment.Gateway.HttpClients.AcquiringBank.Models;
using Payment.Gateway.Messaging.Publisher.Events;
using Payment.Gateway.Models;

namespace Payment.Gateway.Mappers
{
    public class PaymentGatewayMapperProfile : Profile
    {
        public PaymentGatewayMapperProfile()
        {
            CreateMap<Models.CreateTransaction, MongoDb.Models.Transaction>()
                .ForMember(dest => dest.CreatedDateTime, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionId, opt => opt.Ignore());

            CreateMap<MongoDb.Models.Transaction, PaymentTransactionRequest>()
                .ForMember(dest => dest.ProviderId, opt => opt.Ignore());

            CreateMap<Messaging.Subscription.MerchantCreated.MerchantCreatedEvent, MongoDb.Models.Merchant>();

            CreateMap<MongoDb.Models.Transaction,TransactionCreatedEvent>();

            CreateMap<MongoDb.Models.Transaction, TransactionResponse>();
        }
    }
}
