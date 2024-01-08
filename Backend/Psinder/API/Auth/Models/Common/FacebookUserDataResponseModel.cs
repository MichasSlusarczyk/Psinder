using Newtonsoft.Json;

namespace Psinder.API.Auth.Models.Common;

public class FacebookUserDataResponseModel
{
    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("id")]
    public string Id { get; set; }
}
