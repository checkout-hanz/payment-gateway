using System.Reflection;

namespace Payment.Gateway.Configuration
{
    public static class MappersConfiguration
    {
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
