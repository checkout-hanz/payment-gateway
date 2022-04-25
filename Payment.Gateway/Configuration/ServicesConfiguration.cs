using FluentValidation;
using Payment.Gateway.Models;
using Payment.Gateway.Services;
using Payment.Gateway.Utils;
using Payment.Gateway.Validators;

namespace Payment.Gateway.Configuration
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<ITransactionService, TransactionService>();
            services.AddSingleton<IMaskCardNumber, MaskCardNumber>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<IValidator<CreateTransaction>, CreateTransactionValidator>();
            return services;
        }
    }
}
