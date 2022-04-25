using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Payment.Gateway.HttpClients.AcquiringBank;
using Payment.Gateway.HttpClients.AcquiringBank.Models;
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
        private readonly IMaskCardNumber _maskCardNumber;

        public TransactionService(IMerchantRepository merchantRepository, IMapper mapper,
            IDateTimeProvider dateTimeProvider, IPublisher<TransactionCreatedEvent> publisher,
            IValidator<CreateTransaction> createTransactionValidator, IAcquiringBankClient acquiringBankClient,
            ITransactionRepository transactionRepository, IMaskCardNumber maskCardNumber)
        {
            _merchantRepository = merchantRepository;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            _publisher = publisher;
            _createTransactionValidator = createTransactionValidator;
            _acquiringBankClient = acquiringBankClient;
            _transactionRepository = transactionRepository;
            _maskCardNumber = maskCardNumber;
        }

        public async Task<TransactionResponse> MakeTransaction(Models.CreateTransaction createTransaction)
        {
            var validationErrors = new List<ValidationFailure>();

            // check merchant if it exists
            var merchant = await _merchantRepository.GetMerchant(createTransaction.MerchantId);
            if (merchant == null)
            {
                validationErrors.Add(new ValidationFailure(nameof(CreateTransaction.MerchantId), "merchant doesn't exists"));
            }

            // validator
            var validationResult = await _createTransactionValidator.ValidateAsync(createTransaction);
            if (!validationResult.IsValid)
            {
                validationErrors.AddRange(validationResult.Errors);
            }

            if (validationErrors.Any())
            {
                throw new ValidationException(validationErrors);
            }

            // create createTransaction model
            var transaction = _mapper.Map<MongoDb.Models.Transaction>(createTransaction);
            transaction.TransactionId = Guid.NewGuid();
            transaction.CreatedDateTime = _dateTimeProvider.UtcNow;

            // acquiring bank
            var transactionResponse = await _acquiringBankClient.MakePayment(_mapper.Map<PaymentTransactionRequest>(transaction));
            transaction.Status = transactionResponse.Status;

            // insert in to db 
            await _transactionRepository.InsertTransaction(transaction);

            // publish TransactionCreatedEvent
            var transactionCreatedEvent = _mapper.Map<TransactionCreatedEvent>(transaction);
            // mask card number before publishing
            transactionCreatedEvent.CardNumber = _maskCardNumber.Mask(transactionCreatedEvent.CardNumber);

            await _publisher.PublishAsync(transactionCreatedEvent);

            return _mapper.Map<TransactionResponse>(transaction);
        }
    }
}
