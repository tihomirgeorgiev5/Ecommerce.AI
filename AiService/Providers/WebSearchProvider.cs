
namespace AiService.Providers
{
    public class WebSearchProvider : IWebSearchProvider
    {
        public async Task<string> SearchAsync(string query, CancellationToken ct = default)
        {
            //TODO: Replace with Comet/Bing/Google API Keys
            await Task.Delay(50, ct); // Simulate async work
            return $"[Stubbed web result for '{query}']";
        }
    }
}
