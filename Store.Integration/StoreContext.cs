
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
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<StockItem> StockItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("StoreSystem");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockItem>().HasData(
                new StockItem { Id = Guid.NewGuid(), Name = "Chocolate", Barcode = "Chocolate" ,AvailableQuantity = 100, SalesPrice = 10 },
                new StockItem { Id = Guid.NewGuid(), Name = "Cookies", Barcode = "Cookies", AvailableQuantity = 200, SalesPrice = 5 },
                new StockItem { Id = Guid.NewGuid(), Name = "Chips", Barcode = "Chips" , AvailableQuantity = 300, SalesPrice = 1 }
            );
        }
    }
}

