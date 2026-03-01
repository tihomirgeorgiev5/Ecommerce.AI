using System.ComponentModel;

namespace AiService.Repositories
{
    public interface IPgVectorRepository
    {
        Task InsertVectorAsync(
            string productId,
            string name,
            string summary,
            string description,
            string imageFile,
            string brandId,
            string brandName,
            string typeId,
            string typeName,
            DateTimeOffset createdDate,
            float[] embedding);

        Task<IEnumerable<Product>> SearchByVectorAsync(float[] queryVector, int topK = 5);

        Task<IEnumerable<Product>> SearchByKeywordAsync(string keyword, int topK = 5);

        Task<IEnumerable<Product>> SearchByVectorAsync(string query, float[] queryVector, int topK = 5);
    }
}
