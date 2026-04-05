using AiService.Providers;
using AiService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AiService.Endpoints
{
    public static class SemanticSearchEndPoints
    {
        public static IEndpointRouteBuilder MapSemanticSearch(this IEndpointRouteBuilder app)
        {
            app.MapPost("/search/vector", async (
                [FromServices] IEmbeddingProvider embeddings,
                [FromServices] IPgVectorRepository repo,
                QueryReq req,
                CancellationToken ct) =>
            {
                var queryVector = await embeddings.EmbedAsync(req.query, ct);
                var results = await repo.SearchByVectorAsync(queryVector, req.TopK ?? 5);
                return Results.Ok(results);
            });
            //Keyword search
            app.MapPost("/search/keyword", async (
                [FromServices] IPgVectorRepository repo,
                QueryReq req) =>
            {
                var results = await repo.SearchByKeywordAsync(req.query, req.TopK ?? 5);
                return Results.Ok(results);

            });
            //Hybrid search
            app.MapPost("/search/hybrid", async (
                [FromServices] IEmbeddingProvider embeddings,
                [FromServices] IPgVectorRepository repo,
                QueryReq req,
                CancellationToken ct) =>
            {
                var queryVector = await embeddings.EmbedAsync(req.query, ct);
                var results = await repo.SearchByHybridAsync(req.query, queryVector, req.TopK ?? 5);
                return Results.Ok(results);
             });
            return app;
        }
    }

    public record QueryReq(string query, int? TopK);

}
