using Newtonsoft.Json;

namespace CnC.Application.Shared.Responses;

public class ApiResponse
{
    [JsonProperty("conversion_rates")]
    public Dictionary<string, decimal> ConversionRates { get; set; }
}
