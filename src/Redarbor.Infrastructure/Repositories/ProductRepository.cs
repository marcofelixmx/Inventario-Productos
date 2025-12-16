using Microsoft.EntityFrameworkCore;
using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;
using Redarbor.Infrastructure.Persistence;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Redarbor.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly RedarborDbContext _context;
        private readonly IConfiguration _configuration;

        public ProductRepository(RedarborDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddAsync(Product product)
        {
            var sql = "INSERT INTO Product (Name, Description, Price, Stock, CategoryId, Status) VALUES (@Name, @Description, @Price, @Stock, @CategoryId, @Status); SELECT CAST(SCOPE_IDENTITY() as int)";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var id = await connection.QuerySingleAsync<int>(sql, product);
                product.Id = id;
            }
        }

        public async Task UpdateAsync(Product product)
        {
            var sql = "UPDATE Product SET Name = @Name, Description = @Description, Price = @Price, Stock = @Stock, CategoryId = @CategoryId, Status = @Status WHERE Id = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                await connection.ExecuteAsync(sql, product);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM Product WHERE Id = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }
    }
}
