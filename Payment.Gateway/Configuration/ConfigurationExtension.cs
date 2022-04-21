public static class ConfigurationExtension
{
    public static T BindConfigurationSection<T>(this IConfiguration configuration, string section, params object[] constructorArgs) where T : class
    {
        var settings = Activator.CreateInstance(typeof(T), constructorArgs);
        configuration.GetSection(section).Bind(settings);
        return settings as T;
    }
}