using Microsoft.EntityFrameworkCore;
using Product.Microservice.Infrastructure.Entities;
using Product.Microservice.Interfaces.PresistenceInterface;

namespace Product.Microservice.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Catalog> Catalogs { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
