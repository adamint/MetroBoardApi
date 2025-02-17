namespace MetroBoard.Api;

public class HttpUtils
{
    private static readonly HttpClient s_client = new()
    {
        BaseAddress = new Uri("https://api.wmata.com"),
        DefaultRequestHeaders = { { "api_key", Settings.WmataApiKey } }
    };

    public static async Task<T?> GetAsync<T>(string url)
    {
        return await s_client.GetFromJsonAsync<T>(url);
    }
}