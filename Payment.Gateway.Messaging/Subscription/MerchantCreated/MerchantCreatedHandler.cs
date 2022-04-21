using Payment.Gateway.MongoDb.Models;
using Payment.Gateway.MongoDb.Repositories;

namespace Payment.Gateway.Messaging.Subscription.MerchantCreated
{
    public class MerchantCreatedHandler : ISubscriptionHandler<MerchantCreatedEvent>
    {
        private readonly IMerchantRepository _merchantRepository;

        public MerchantCreatedHandler(IMerchantRepository merchantRepository)
        {
            _merchantRepository = merchantRepository;
        }

        public async Task Handle(MerchantCreatedEvent message)
        {
            // automapper
            
            await _merchantRepository.InsertMerchant(new Merchant()
            {
                MerchantId = message.MerchantId,
                Email = message.Email,
                Name = message.Name
            });
        }
    }
}
