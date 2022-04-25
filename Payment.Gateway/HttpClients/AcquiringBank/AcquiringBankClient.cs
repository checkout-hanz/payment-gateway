using System.Net;
using System.Text.Json;
using FluentValidation.Results;
using Payment.Gateway.Configuration;
using Payment.Gateway.HttpClients.AcquiringBank.Models;

namespace Payment.Gateway.HttpClients.AcquiringBank;

public class AcquiringBankClient : IAcquiringBankClient
{
    private readonly HttpClient _httpClient;
    private readonly AcquiringBankConfig _acquiringBankConfig;
    public AcquiringBankClient(HttpClient httpClient, AcquiringBankConfig acquiringBankConfig)
    {
        _httpClient = httpClient;
        _acquiringBankConfig = acquiringBankConfig;
    }

    public async Task<PaymentTransactionResponse> MakePayment(PaymentTransactionRequest request)
    {
        request.ProviderId = _acquiringBankConfig.ProviderId;

        var response = await _httpClient.PostAsJsonAsync($"{_httpClient.BaseAddress}payment", request);
        var content = await response.Content.ReadAsStringAsync();
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return Deserialize<PaymentTransactionResponse>(content);
            }

            throw;
        }

        return Deserialize<PaymentTransactionResponse>(content);
    }

    private T Deserialize<T>(string content)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        return JsonSerializer.Deserialize<T>(content, options);
    }
}