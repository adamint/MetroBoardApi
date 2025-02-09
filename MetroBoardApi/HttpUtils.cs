namespace MetroBoardApi;

public class HttpUtils
{
    private static readonly string WmataApiKey = Environment.GetEnvironmentVariable("WMATA_API_KEY") ?? throw new ArgumentException("WMATA_API_KEY environment variable not set");

    private static readonly HttpClient s_client = new()
    {
        BaseAddress = new Uri("https://api.wmata.com"),
        DefaultRequestHeaders = { { "api_key", WmataApiKey } }
    };

    public static async Task<T?> GetAsync<T>(string url)
    {
        return await s_client.GetFromJsonAsync<T>(url);
    }
}