namespace Payment.Gateway.Services
{
    public interface IMaskCardNumber
    {
        string Mask(string cardNumber);
    }
}
