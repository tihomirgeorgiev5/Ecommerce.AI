using AiService.Models;
using Dapper;
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
        public async Task InsertProductVectorAsync(
            string productId,
            string name,
            string summary,
            string description,
            string imageFile,
            string brandId,
            string brandName,
            string typeId,
            string typeName,
            decimal price,
            DateTimeOffset createdDate,
            float[] embedding)
        {
            // Decide target columns based on the embedding dimension
            string embeddingColumn = embedding.Length switch
            {
                1536 => "embedding_1536",
                768 => "embedding_768",
                3072 => "embedding_3072", // future proof
                _ => throw new InvalidOperationException(
                    $"Unsupported embedding size {embedding.Length}. Expected 768, 1536 or 3072.")
            };
            await using var conn = await _dataSource.OpenConnectionAsync();

            string sql = $@"
                INSERT INTO product_vectors
                (product_id, name, summary, description, image_file,
                 brand_id, brand_name, type_id, type_name,
                 price, created_date, {embeddingColumn})
                VALUES 
                (@ProductId, @Name, @Summary, @Description, @ImageFile,
                    @BrandId, @BrandName, @TypeId, @TypeName,
                    @Price, @CreatedDate, @Embedding)
                  ON CONFLICT (product_id) DO UPDATE 
                  SET {embeddingColumn} = EXCLUDED.{embeddingColumn},
                      name = EXCLUDED.name,
                      summary = EXCLUDED.summary,
                      description = EXCLUDED.description,
                      image_file = EXCLUDED.image_file,
                      brand_id = EXCLUDED.brand_id,
                      brand_name = EXCLUDED.brand_name,
                      type_id = EXCLUDED.type_id,
                      type_name = EXCLUDED.type_name,
                      price = EXCLUDED.price,
                      created_date = EXCLUDED.created_date; ";

            await conn.ExecuteAsync(sql, new
            {
                ProductId = productId,
                Name = name,
                Summary = summary,
                Description = description,
                ImageFile = imageFile,
                BrandId = brandId,
                BrandName = brandName,
                TypeId = typeId,
                Price = price,
                CreatedDate = createdDate,
                TypeName = typeName,
                Embedding = embedding
            });
        }

        public async Task<IEnumerable<Product>> SearchByVectorAsync(float[] queryVector, int topK = 5)
        {
            await using var conn = await _dataSource.OpenConnectionAsync();

            //Decide Embedding Column
        string embeddingColumn = queryVector.Length switch
        {
            1536 => "embedding_1536",
            768 => "embedding_768",
            3072 => "embedding_3072", // future proof
            _ => throw new InvalidOperationException(
                $"Unsupported embedding size {queryVector.Length}. Expected 768, 1536 or 3072.")
        };
            string sql = $@"
    SELECT
      product_id AS id,
      name,
      description,
      image_file AS ""imageFile"",
      price,
      brand_id AS brandName,
      brand_name AS brandName,
      type_id AS typeId,
      type_name AS typeName,
      1 - ({embeddingColumn} <=> @QueryVector::vector) AS similarity
    FROM prooduct_vectors
    ORDER BY similarity DESC
    LIMIT @TopK";
            var rows = await conn.QueryAsync<Product>(sql, new { QueryVector = queryVector, TopK = topK });
            return rows.Select(MapToProduct);
        }


        public Task<IEnumerable<Product>> SearchByKeywordAsync(string keyword, int topK = 5)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<Product>> SearchByHybridAsync(string query, float[] queryVector, int topK = 5)
        {
            throw new NotImplementedException();
        }

        private static Product MapToProduct(dynamic row)
        {
            return new Product(
                Id: row.id,
                Name: row.name,
                Description: row.description,
                ImageFile: row.imageFile,
                Price: row.price,
                Brand: new Brand(row.brandId, row.brandName),
                Type: new Models.Type(row.typeId, row.typeName)
            );
        }


    }
}
