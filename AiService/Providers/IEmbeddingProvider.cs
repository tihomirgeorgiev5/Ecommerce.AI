namespace AiService.Providers
{
    public interface IEmbeddingProvider
    {
        Task<float[]> EmbedAsync(string text, CancellationToken cancellationToken);

        Task<float[][]> EmbedBatchAsync(IEnumerable<string> texts, CancellationToken cancellationToken);

        int Dimension { get; } // eg 768, 1536
    }
}
