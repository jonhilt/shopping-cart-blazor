using Microsoft.EntityFrameworkCore;
using ShoppingCartWithBackend.Models;

namespace ShoppingCartWithBackend.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ShoppingCart> Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<ShoppingCart>()
                .HasMany(x => x.LineItems)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}