using Newtonsoft.Json;

namespace Psinder.IntegrationTests;

public static class HttpHelpers
{
    public static async Task<T?> GetResponse<T>(this HttpResponseMessage httpResponseMessage)
    {
        var res = await httpResponseMessage.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(res);
    }
}
