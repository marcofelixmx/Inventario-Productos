using Microsoft.EntityFrameworkCore;
using Redarbor.Core.Domain;

namespace Redarbor.Infrastructure.Persistence
{
    public class RedarborDbContext : DbContext
    {
        public RedarborDbContext(DbContextOptions<RedarborDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<InventoryMovement> InventoryMovements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
