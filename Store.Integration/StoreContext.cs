
using Domain.StoreSystem.models;
using Microsoft.EntityFrameworkCore;

namespace Store.Integration
{
    public class StoreContext : DbContext
    {
        
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }
        public DbSet<Domain.StoreSystem.models.Store> Stores { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<CachedProduct> CachedProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("StoreSystem");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data 
            modelBuilder.Entity<StockItem>().HasData(
                new StockItem { Id = Guid.NewGuid(), Name = "Chocolate", Barcode = "Chocolate" ,AvailableQuantity = 100, SalesPrice = 1.50f },
                new StockItem { Id = Guid.NewGuid(), Name = "Cookies", Barcode = "Cookies", AvailableQuantity = 200, SalesPrice = 2.00f },
                new StockItem { Id = Guid.NewGuid(), Name = "Chips", Barcode = "Chips" , AvailableQuantity = 300, SalesPrice = 1.00f }
            );
        }
    }
}

