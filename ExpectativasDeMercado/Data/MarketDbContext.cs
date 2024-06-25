using Microsoft.EntityFrameworkCore;

namespace ExpectativasDeMercado.Data
{
    public class MarketDbContext : DbContext
    {
        public DbSet<MarketExpectation> MarketExpectations { get; set; }
        public DbSet<SelicExpectation> SelicExpectations { get; set; }

        public MarketDbContext(DbContextOptions<MarketDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MarketExpectation>(entity =>
            {
                entity.HasKey(e => new { e.Indicador, e.Data });
            });

            modelBuilder.Entity<SelicExpectation>(entity =>
            {
                entity.HasKey(e => new { e.Indicador, e.Data });
            });
        }
    }
}
