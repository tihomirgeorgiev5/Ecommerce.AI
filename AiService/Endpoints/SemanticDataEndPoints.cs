using AiService.Models;
using AiService.Providers;
using AiService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AiService.Endpoints
{
    public static class SemanticDataEndPoints
    {
        public static IEndpointRouteBuilder MapSemanticData(this IEndpointRouteBuilder app)
        {
            app.MapPost("/semantic/load", async (
                [FromServices] IEmbeddingProvider embeddings,
                [FromServices] IPgVectorRepository repo,
                [FromServices] IHttpClientFactory http,
                CancellationToken ct
                ) =>
            {
                var client = http.CreateClient("CatalogApi");
                var response = await client.GetFromJsonAsync<CatalogResponse>(
                    "api/v1/Catalog/GetAllProducts?pageIndex=1&pageSize=100", ct);

                if (response?.Data == null || response.Data.Count == 0)
                    return Results.BadRequest("No products found");

                const int batchSize = 10;
                int processed = 0;

                for (int i = 0; i < response.Data.Count; i += batchSize)
                {
                    var batch = response.Data.Skip(i).Take(batchSize).ToList();

                    // Prepare texts for embedding
                    var texts = batch.Select(p => $"{p.Name} {p.Summary} {p.Description} {p.Brand?.Name} {p.Type?.Name}").ToArray();

                    float[][] vectors;
                    try
                    {
                        //Embedding
                        vectors = await embeddings.EmbedBatchAsync(texts, ct);
                    }
                    catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        // Handle rate limiting (e.g., wait and retry)
                        await Task.Delay(1000, ct); // Wait for 1 second before retrying
                        vectors = await embeddings.EmbedBatchAsync(texts, ct);
                    }
                    for (int j = 0; j < batch.Count; j++)
                    {
                        var product = batch[j];
                        var vector = vectors[j]; // length tells repo which column to use
                        await repo.InsertProductVectorAsync(
                           product.Id,
                           product.Name,
                            product.Summary,
                            product.Description,
                            product.ImageFile,
                            product.Brand?.Id ?? string.Empty,
                            product.Brand?.Name ?? string.Empty,
                            product.Type?.Id ?? string.Empty,
                            product.Type?.Name ?? string.Empty,
                            product.Price,
                            product.CreatedDate,
                            vector
                            );
                        processed++;
                    }
                    //Throttle between batches
                    await Task.Delay(500, ct);
                }
                return Results.Ok(new { Count = processed });
            });
             return app;
        }
    }
}
