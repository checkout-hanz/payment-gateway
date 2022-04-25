using FluentValidation;
using Payment.Gateway.Models;
using Payment.Gateway.Utils;

namespace Payment.Gateway.Validators
{
    public class CreateTransactionValidator : AbstractValidator<CreateTransaction>
    {
        public CreateTransactionValidator(IDateTimeProvider dateTimeProvider)
        {
            RuleFor(m => m.MerchantId).NotEmpty();
            RuleFor(m => m.Amount).GreaterThan(0);
            RuleFor(m => m.CardNumber).NotEmpty().CreditCard();
            RuleFor(m => m.ExpiryYear).GreaterThanOrEqualTo(dateTimeProvider.UtcNow.Year);
            When(x=>x.ExpiryYear == dateTimeProvider.UtcNow.Year, () => {
                RuleFor(m => m.ExpiryMonth).GreaterThanOrEqualTo(DateTime.Now.Month);
            });

            RuleFor(m => m.Currency).NotEmpty();
            RuleFor(m => m.CVV).NotEmpty();
        }
    }
}
