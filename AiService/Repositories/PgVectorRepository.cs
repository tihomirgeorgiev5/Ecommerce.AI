using AiService.Models;
using Npgsql;

namespace AiService.Repositories
{
    public class PgVectorRepository : IPgVectorRepository
    {
        private readonly NpgsqlDataSource _dataSource;

        public PgVectorRepository(NpgsqlDataSource dataSource)
        {
            _dataSource = dataSource;
        }
        public Task InsertVectorAsync(string productId, string name, string summary, string description, string imageFile, string brandId, string brandName, string typeId, string typeName, DateTimeOffset createdDate, float[] embedding)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> SearchByKeywordAsync(string keyword, int topK = 5)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> SearchByVectorAsync(float[] queryVector, int topK = 5)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> SearchByVectorAsync(string query, float[] queryVector, int topK = 5)
        {
            throw new NotImplementedException();
        }
    }
}
