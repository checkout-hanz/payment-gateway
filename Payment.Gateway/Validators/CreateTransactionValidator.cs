using FluentValidation;
using Payment.Gateway.Models;

namespace Payment.Gateway.Validators
{
    public class CreateTransactionValidator : AbstractValidator<CreateTransaction>
    {
        public CreateTransactionValidator()
        {
            RuleFor(m => m.MerchantId).NotEmpty();
            RuleFor(m => m.Amount).GreaterThan(0);
            RuleFor(m => m.CardNumber).CreditCard();
            RuleFor(m => m.ExpiryYear).GreaterThanOrEqualTo(DateTime.Now.Year);
            When(x=>x.ExpiryYear == DateTime.Now.Year, () => {
                RuleFor(m => m.ExpiryMonth).GreaterThanOrEqualTo(DateTime.Now.Month);
            });

            RuleFor(m => m.Currency).NotEmpty();
            RuleFor(m => m.CVV).NotEmpty();
        }
    }
}
