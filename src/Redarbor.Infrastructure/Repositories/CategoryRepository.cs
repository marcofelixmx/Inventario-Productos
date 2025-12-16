using Microsoft.EntityFrameworkCore;
using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;
using Redarbor.Infrastructure.Persistence;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Redarbor.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly RedarborDbContext _context;
        private readonly IConfiguration _configuration;

        public CategoryRepository(RedarborDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddAsync(Category category)
        {
            var sql = "INSERT INTO Category (Name) VALUES (@Name); SELECT CAST(SCOPE_IDENTITY() as int)";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var id = await connection.QuerySingleAsync<int>(sql, category);
                category.Id = id;
            }
        }

        public async Task UpdateAsync(Category category)
        {
            var sql = "UPDATE Category SET Name = @Name WHERE Id = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                await connection.ExecuteAsync(sql, category);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var sql = "DELETE FROM Category WHERE Id = @Id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }
    }
}
