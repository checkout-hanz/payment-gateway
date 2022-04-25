namespace Payment.Gateway.Configuration
{
    public interface IHttpClientConfig
    {
        public string BaseUrl { get; set; }
        public int TimeoutSeconds { get; set; }
    }

    public class HttpClientConfig : IHttpClientConfig
    {
        public string BaseUrl { get; set; }
        public int TimeoutSeconds { get; set; }
    }

    public class AcquiringBankConfig : HttpClientConfig
    {
        public static string Config = "AcquiringBank";
        public string ProviderId { get; set; }
        public string PaymentUrl { get; set; }
    }
}
