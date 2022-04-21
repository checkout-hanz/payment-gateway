//using AutoMapper;
//using FluentAssertions;
//using FluentValidation;
//using FluentValidation.Results;
//using Payment.Gateway.Mappers;
//using Payment.Gateway.Messaging.Publisher;
//using Payment.Gateway.Messaging.Publisher.Events;
//using Payment.Gateway.Models;
//using Payment.Gateway.MongoDb.Repositories;
//using Payment.Gateway.Services;
//using Payment.Gateway.Utils;
//using MongoDB.Driver;
//using Moq;
//using Xunit;

//namespace Payment.Gateway.UnitTests
//{
//    public class MerchantServiceTests
//    {
//        private readonly IMerchantService _merchantService;

//        private readonly Mock<IMerchantRepository> _merchantRepository;
//        private readonly Mock<IMapper> _mapper;
//        private readonly Mock<IDateTimeProvider> _dateTimeProvider;
//        private readonly Mock<IValidator<CreateMerchant>> _createMerchantValidator;
//        private readonly Mock<IPublisher<MerchantCreatedEvent>> _publisher;

//        public MerchantServiceTests()
//        {
//            _merchantRepository = new Mock<IMerchantRepository>();
//            _mapper = new Mock<IMapper>();

//            var configuration = new MapperConfiguration(configure =>
//            {
//                configure.AddProfile(new MerchantMapperProfile());
//            });

//            configuration.AssertConfigurationIsValid();

//            var mapper = new Mapper(configuration);

//            _dateTimeProvider = new Mock<IDateTimeProvider>();
//            _createMerchantValidator = new Mock<IValidator<CreateMerchant>>();
//            _publisher = new Mock<IPublisher<MerchantCreatedEvent>>();

//            _dateTimeProvider.Setup(x => x.UtcNow).Returns(new DateTime(2000, 1, 1));
//            _publisher.Setup(x => x.PublishAsync(It.IsAny<MerchantCreatedEvent>())).Returns(Task.CompletedTask);


//            _merchantService = new MerchantService(_merchantRepository.Object, mapper, _dateTimeProvider.Object,
//                _publisher.Object, _createMerchantValidator.Object);
//        }

//        [Fact]
//        public async Task WhenMerchantNotInRepo_ExpectNull()
//        {
//            _merchantRepository.Setup(x => x.GetMerchant(It.IsAny<Guid>())).ReturnsAsync((MongoDb.Models.Merchant)null);

//            var merchant = await _merchantService.GetMerchant(Guid.NewGuid().ToString());

//            merchant.Should().BeNull();
//        }

//        [Fact]
//        public async Task CreateMerchantWithInvalid_ExpectException()
//        {
//            _createMerchantValidator
//                .Setup(x => x.ValidateAsync(It.IsAny<CreateMerchant>(), It.IsAny<CancellationToken>())).ReturnsAsync(
//                    new ValidationResult()
//                    {
//                        Errors = { new ValidationFailure("xyx", "errr") }
//                    });


//            Func<Task> act = async () => await _merchantService.AddMerchant(new CreateMerchant());

//            await act.Should().ThrowAsync<ValidationException>();
//            _publisher.Verify(x => x.PublishAsync(It.IsAny<MerchantCreatedEvent>()), Times.Never);
//        }

//        [Fact]
//        public async Task CreateMerchantWithValid_ExpectEventPublished()
//        {
//            _createMerchantValidator
//                .Setup(x => x.ValidateAsync(It.IsAny<CreateMerchant>(), It.IsAny<CancellationToken>())).ReturnsAsync(
//                    new ValidationResult());

//            await _merchantService.AddMerchant(new CreateMerchant());

//            _publisher.Verify(x => x.PublishAsync(It.IsAny<MerchantCreatedEvent>()), Times.Once);
//        }

//    }
//}