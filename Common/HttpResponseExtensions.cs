using Newtonsoft.Json;

namespace Common.Extensions;

public static class HttpResponseExtensions
{
    public static async Task<TResponse> DeserializeContent<TResponse>(this HttpResponseMessage response, JsonSerializerSettings jsonSerializerSettings)
    {
        if (response.Content == null)
        {
            return default;
        }

        var jsonSerializer = JsonSerializer.Create(jsonSerializerSettings);

        await using var stream = await response.Content
            .ReadAsStreamAsync()
            .ConfigureAwait(false);

        using var reader = new StreamReader(stream);
        using var jsonReader = new JsonTextReader(reader);

        return jsonSerializer.Deserialize<TResponse>(jsonReader);
    }
}
