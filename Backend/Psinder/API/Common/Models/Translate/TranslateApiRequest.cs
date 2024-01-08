using Newtonsoft.Json;

namespace Psinder.API.Common.Models.Translate;

public class TranslateApiRequest
{
    [JsonProperty("q")]
    public string Q { get; set; }

    [JsonProperty("source")]
    public string Source { get; set; }

    [JsonProperty("target")]
    public string Target { get; set; }

    [JsonProperty("format")]
    public string Format { get; set; }

    [JsonProperty("api_key")]
    public string ApiKey { get; set; }
}
