namespace AiService.Models
{
    public record ProductDto
        (
        string Id,
        string Name,
        string Summary,
        string Description,
        string ImageFile,
        ProductBrand Brands,
        ProductType Types,
        decimal Price,
        DateTimeOffset CreatedDate

        
        );
    
}
