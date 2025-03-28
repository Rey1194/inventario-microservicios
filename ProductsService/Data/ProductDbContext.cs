using Microsoft.EntityFrameworkCore;
using ProductsService.Models;

namespace ProductsService.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Description = "Laptop Dell", Category = "Electronics", ImageUrl = "laptop.jpg", Price = 1200.50m, Stock = 10 },
                new Product { Id = 2, Name = "Mouse", Description = "Mouse inalámbrico", Category = "Electronics", ImageUrl = "mouse.jpg", Price = 25.99m, Stock = 50 }
            );
        }
    }
}
