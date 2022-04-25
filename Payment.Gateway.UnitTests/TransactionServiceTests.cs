using AutoMapper;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Payment.Gateway.HttpClients.AcquiringBank;
using Payment.Gateway.HttpClients.AcquiringBank.Models;
using Payment.Gateway.Mappers;
using Payment.Gateway.Messaging.Publisher;
using Payment.Gateway.Messaging.Publisher.Events;
using Payment.Gateway.Models;
using Payment.Gateway.MongoDb.Models;
using Payment.Gateway.MongoDb.Repositories;
using Payment.Gateway.Services;
using Payment.Gateway.Utils;
using Xunit;
using Transaction = Payment.Gateway.MongoDb.Models.Transaction;

namespace Payment.Gateway.UnitTests
{
    public class TransactionServiceTests
    {
        private readonly ITransactionService _transactionService;

        private readonly Mock<IMerchantRepository> _merchantRepository;
        private readonly Mock<IDateTimeProvider> _dateTimeProvider;
        private readonly Mock<IPublisher<TransactionCreatedEvent>> _publisher;
        private readonly Mock<IValidator<CreateTransaction>> _validator;
        private readonly Mock<IAcquiringBankClient> _acquiringBankClient;
        private readonly Mock<ITransactionRepository> _transactionRepository;
        private readonly Mock<IMaskCardNumber> _maskCardNumber;

        public TransactionServiceTests()
        {
            _merchantRepository = new Mock<IMerchantRepository>();

            var configuration = new MapperConfiguration(configure =>
            {
                configure.AddProfile(new PaymentGatewayMapperProfile());
            });
            configuration.AssertConfigurationIsValid();
            var mapper = new Mapper(configuration);

            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _publisher = new Mock<IPublisher<TransactionCreatedEvent>>();
            _validator = new Mock<IValidator<CreateTransaction>>();
            _acquiringBankClient = new Mock<IAcquiringBankClient>();
            _transactionRepository = new Mock<ITransactionRepository>();
            _maskCardNumber = new Mock<IMaskCardNumber>();

            _transactionService = new TransactionService(_merchantRepository.Object, mapper,
                _dateTimeProvider.Object, _publisher.Object, _validator.Object, _acquiringBankClient.Object,
                _transactionRepository.Object, _maskCardNumber.Object);

            _merchantRepository.Setup(x => x.GetMerchant(It.IsAny<Guid>())).ReturnsAsync(new Merchant());

            _validator.Setup(x => x.ValidateAsync(It.IsAny<CreateTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
        }

        [Fact]
        public async Task WhenInvalidMerchant_ThrowsValidationException()
        {
            _merchantRepository.Setup(x => x.GetMerchant(It.IsAny<Guid>())).ReturnsAsync((Merchant)null);

            var act = async () => await _transactionService.MakeTransaction(new CreateTransaction());

            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task WhenValidMerchantButInvalidData_ThrowsValidationException()
        {
            _merchantRepository.Setup(x => x.GetMerchant(It.IsAny<Guid>())).ReturnsAsync(new Merchant());

            _validator.Setup(x => x.ValidateAsync(It.IsAny<CreateTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult()
                {
                    Errors = { new ValidationFailure("xys", "dsfsfsdf") }
                });

            var act = async () => await _transactionService.MakeTransaction(new CreateTransaction());

            await act.Should().ThrowAsync<ValidationException>();
        }

        [Theory]
        [InlineData(PaymentTransactionStatus.Success)]
        [InlineData(PaymentTransactionStatus.Failed)]
        public async Task WhenValidMerchantWithValidData_TransactionIsPersistedAndEventIsPublishedWithCorrectStatus(PaymentTransactionStatus status)
        {
            Guid merchantId = Guid.NewGuid();

            _acquiringBankClient.Setup(x => x.MakePayment(It.IsAny<PaymentTransactionRequest>()))
                .ReturnsAsync(new PaymentTransactionResponse()
                {
                    Status = status
                });


            _transactionRepository.Setup(x => x.InsertTransaction(It.IsAny<Transaction>())).Returns(Task.CompletedTask);
            _maskCardNumber.Setup(x => x.Mask(It.IsAny<string>())).Returns("5555********4444");
            _publisher.Setup(x => x.PublishAsync(It.IsAny<TransactionCreatedEvent>())).Returns(Task.CompletedTask);


            await _transactionService.MakeTransaction(new CreateTransaction()
            {
                MerchantId = merchantId,
                CardNumber = "5555222233334444",
                Amount = 10
            });


            _transactionRepository.Verify(x => x.InsertTransaction(It.Is<Transaction>(x=>x.CardNumber == "5555222233334444")));
            _maskCardNumber.Verify(x => x.Mask(It.IsAny<string>()));
            _publisher.Verify(x => x.PublishAsync(It.Is<TransactionCreatedEvent>(x =>
                x.MerchantId == merchantId && x.CardNumber == "5555********4444" && x.Amount == 10)));
        }
    }
}
