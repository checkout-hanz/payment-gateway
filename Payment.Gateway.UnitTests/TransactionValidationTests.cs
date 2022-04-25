using System.Collections;
using FluentAssertions;
using FluentValidation;
using Moq;
using Payment.Gateway.Models;
using Payment.Gateway.Utils;
using Payment.Gateway.Validators;
using Xunit;

namespace Payment.Gateway.UnitTests
{
    public class TransactionValidationTests
    {
        private readonly IValidator<CreateTransaction> _validator;

        public TransactionValidationTests()
        {
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(x => x.UtcNow).Returns(new DateTime(2022, 03, 01));

            _validator = new CreateTransactionValidator(dateTimeProvider.Object);
        }

        [Theory, ClassData(typeof(TestData))]

        public async Task CheckValidationExp(CreateTransaction transaction, List<string> errorProperties, bool expected)
        {
            var validationResult = await _validator.ValidateAsync(transaction);

            var validationErrorProperties = validationResult.Errors?.Select(e => e.PropertyName).ToList();

            validationResult.IsValid.Should().Be(expected);
            validationErrorProperties.Should().Equal(errorProperties);
        }
    }

    public class TestData : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[]
            {
                new CreateTransaction()
                {
                    CardNumber = "55555555554444",
                    Amount = 0,
                    CVV = "",
                    Currency = "",
                    ExpiryMonth = 5,
                    ExpiryYear = 2022,
                    MerchantId = Guid.NewGuid()
                },
                new List<string>
                {
                    "Amount", "CardNumber", "Currency", "CVV"
                },
                false
            },

            new object[]
            {
                new CreateTransaction()
                {
                    CardNumber = "",
                    Amount = 1,
                    CVV = "454",
                    Currency = "GBP",
                    ExpiryMonth = 5,
                    ExpiryYear = 2020,
                    MerchantId = Guid.NewGuid()
                },
                new List<string>
                {
                    "CardNumber", "ExpiryYear"
                },
                false
            },

            new object[]
            {
                new CreateTransaction()
                {
                    CardNumber = "fsdfsdfsdf",
                    Amount = 1,
                    CVV = "454",
                    Currency = "GBP",
                    ExpiryMonth = 5,
                    ExpiryYear = 2022,
                    MerchantId = Guid.NewGuid()
                },
                new List<string>
                {
                    "CardNumber",
                },
                false
            },
            new object[]
            {
                new CreateTransaction()
                {
                    CardNumber = "5555555555554444",
                    Amount = 0,
                    CVV = "454",
                    Currency = "GBP",
                    ExpiryMonth = 5,
                    ExpiryYear = 2022,
                    MerchantId = Guid.NewGuid()
                },
                new List<string>
                {
                    "Amount"
                },
                false
            },
            new object[]
            {
                new CreateTransaction()
                {
                    CardNumber = "5555555555554444",
                    Amount = 1,
                    CVV = "454",
                    Currency = "GBP",
                    ExpiryMonth = 3,
                    ExpiryYear = 2022,
                    MerchantId = Guid.NewGuid()
                },
                new List<string>
                {
                    "ExpiryMonth"
                },
                false
            },
            new object[]
            {
                new CreateTransaction()
                {
                    CardNumber = "5555555555554444",
                    Amount = 1,
                    CVV = "454",
                    Currency = "GBP",
                    ExpiryMonth = 5,
                    ExpiryYear = 2022,
                    MerchantId = Guid.NewGuid()
                },
                new List<string>(),
                true
            },
        };

        public IEnumerator<object[]> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
