using AutoMapper;

namespace Payment.Gateway.Mappers
{
    public class PaymentGatewayMapperProfile : Profile
    {
        public PaymentGatewayMapperProfile()
        {
            //CreateMap<Models.CreateMerchant, MongoDb.Models.Merchant>()
            //    .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            //    .ForMember(dest => dest.MerchantId, opt => opt.Ignore());

            //CreateMap<Models.Merchant, MongoDb.Models.Merchant>()
            //    .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            //    .ReverseMap();

            CreateMap<Messaging.Subscription.MerchantCreated.MerchantCreatedEvent, MongoDb.Models.Merchant>();
        }
    }
}
