public static class ConfigurationExtension
{
    public static T BindConfigurationSection<T>(this IServiceCollection services, IConfiguration configuration, string section, params object[] constructorArgs) where T : class
    {
        var settings = Activator.CreateInstance(typeof(T), constructorArgs);
        configuration.GetSection(section).Bind(settings);
        //services.Configure<T>(configuration.GetSection(section));
        var obj =  settings as T;
        services.AddSingleton<T>(obj);

        return obj;
    }
}