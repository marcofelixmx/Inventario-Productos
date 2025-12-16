using Redarbor.Core.Domain;
using Redarbor.Core.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Redarbor.Infrastructure.Repositories
{
    public class InventoryMovementRepository : IInventoryMovementRepository
    {
        private readonly IConfiguration _configuration;

        public InventoryMovementRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task AddAsync(InventoryMovement inventoryMovement)
        {
            var sql = "INSERT INTO InventoryMovement (ProductId, Quantity, MovementDate) VALUES (@ProductId, @Quantity, @MovementDate); SELECT CAST(SCOPE_IDENTITY() as int)";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var id = await connection.QuerySingleAsync<int>(sql, inventoryMovement);
                inventoryMovement.Id = id;
            }
        }
    }
}
