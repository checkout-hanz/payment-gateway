using AutoMapper;
using Payment.Gateway.MongoDb.Models;
using Payment.Gateway.MongoDb.Repositories;

namespace Payment.Gateway.Messaging.Subscription.MerchantCreated
{
    public class MerchantCreatedHandler : ISubscriptionHandler<MerchantCreatedEvent>
    {
        private readonly IMerchantRepository _merchantRepository;
        private readonly IMapper _mapper;

        public MerchantCreatedHandler(IMerchantRepository merchantRepository, IMapper mapper)
        {
            _merchantRepository = merchantRepository;
            _mapper = mapper;
        }

        public async Task Handle(MerchantCreatedEvent message)
        {
            await _merchantRepository.InsertMerchant(_mapper.Map<Merchant>(message));
        }
    }
}
