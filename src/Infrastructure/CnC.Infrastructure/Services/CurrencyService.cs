using CnC.Application.Abstracts.Services;
using CnC.Application.Shared.Responses;
using Newtonsoft.Json;

namespace CnC.Infrastructure.Services;

public class CurrencyService : ICurrencyService
{
    private readonly HttpClient _httpClient;
    private const string ApiKey = "8c109feefd2aa6abee7c480e"; // API Anahtarınızı buraya ekleyin
    private const string ApiUrl = "https://v6.exchangerate-api.com/v6/{0}/latest/{1}"; // API URL'si (örnek)

    public CurrencyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> ConvertAsync(decimal amount, string fromCurrency, string toCurrency)
    {
        var url = string.Format(ApiUrl, ApiKey, fromCurrency); // API URL'sini oluştur

        var response = await _httpClient.GetStringAsync(url);
        var data = JsonConvert.DeserializeObject<ApiResponse>(response); // JSON yanıtını çözümle

        if (data?.ConversionRates != null && data.ConversionRates.ContainsKey(toCurrency))
        {
            decimal rate = data.ConversionRates[toCurrency];
            return amount * rate;
        }

        throw new Exception("Döviz kuru verisi alınamadı.");
    }
}
