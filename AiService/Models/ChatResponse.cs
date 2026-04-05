namespace AiService.Models
{
    public record ChatResponse(
        string Answer,
        IEnumerable<SourceResult> Sources = null,
        IEnumerable<Product>? Products = null
        );
   
}
