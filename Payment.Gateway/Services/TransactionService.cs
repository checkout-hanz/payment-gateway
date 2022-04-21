using AutoMapper;
using FluentValidation;
using Payment.Gateway.Messaging.Publisher;
using Payment.Gateway.Messaging.Publisher.Events;
using Payment.Gateway.Models;
using Payment.Gateway.MongoDb.Repositories;
using Payment.Gateway.Utils;

namespace Payment.Gateway.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IMerchantRepository _merchantRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IValidator<CreateTransaction> _createTransactionValidator;
        private readonly IPublisher<TransactionCreatedEvent> _publisher;
        private readonly IAcquiringBankClient _acquiringBankClient;


        public TransactionService(IMerchantRepository merchantRepository, IMapper mapper, IDateTimeProvider dateTimeProvider, IPublisher<TransactionCreatedEvent> publisher, IValidator<CreateTransaction> createTransactionValidator, IAcquiringBankClient acquiringBankClient)
        {
           _merchantRepository = merchantRepository;
           _mapper = mapper;
           _dateTimeProvider = dateTimeProvider;
           _publisher = publisher;
           _createTransactionValidator = createTransactionValidator;
           _acquiringBankClient = acquiringBankClient;;
        }

        public async Task MakeTransaction(Models.CreateTransaction transaction)
        {
            var merchant  = _merchantRepository.GetMerchant(transaction.MerchantId);
            if(merchant == null)
            {
                throw new InvalidOperationException("merchant doesn't exists");
            }

            // validator
            var validationResult = await _createTransactionValidator.ValidateAsync(transaction);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // acquiring bank
            var transactionModel = _mapper.Map<MongoDb.Models.Transaction>(merchant);
            transactionModel.TransactionId = Guid.NewGuid();

            // insert in to db 
            _transactionRepository.InsertTransaction(transactionModel);

            // publish TransactionCreatedEvent
            var transactionCreatedEvent = _mapper.Map<TransactionCreatedEvent>(transactionModel);
            await _publisher.PublishAsync(transactionCreatedEvent);

        }


       
    }
}
