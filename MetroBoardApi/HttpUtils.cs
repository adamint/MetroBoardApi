namespace MetroBoardApi;

public class HttpUtils
{
    private const string WmataApiKey = "";

    private static HttpClient s_client = new()
    {
        BaseAddress = new Uri("https://api.wmata.com"),
        DefaultRequestHeaders = { { "api_key", WmataApiKey } }
    };

    public static async Task<T?> GetAsync<T>(string url)
    {
        return await s_client.GetFromJsonAsync<T>(url);
    }
}