
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
        public DbSet<OrderSupplier> OrderSuppliers { get; set; } 
        
        public DbSet<OrderSupplierCachedProduct> OrderSupplierCachedProducts { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<CachedProduct> CachedProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("StoreSystem");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var chocolateProduct = new CachedProduct()
                { Id = Guid.NewGuid(), Name = "Chocolate", Barcode = "Chocolate", CurrentPrice = 1.50f };
            var cookiesProduct = new CachedProduct
                { Id = Guid.NewGuid(), Name = "Cookies", Barcode = "Cookies", CurrentPrice = 2.00f };
            var chipsProduct = new CachedProduct
                { Id = Guid.NewGuid(), Name = "Chips", Barcode = "Chips", CurrentPrice = 1.00f };

            var chocolateStockItem = new StockItem()
            {
                Id = Guid.NewGuid(), Name = "Chocolate", AvailableQuantity = 100,
                CachedProductId = chocolateProduct.Id
            };
            var cookiesStockItem = new StockItem()
            {
                Id = Guid.NewGuid(), Name = "Cookies", AvailableQuantity = 100,
                CachedProductId = cookiesProduct.Id
            };
            var chipsStockItem = new StockItem()
            {
                Id = Guid.NewGuid(), Name = "Chips", AvailableQuantity = 100,
                CachedProductId = chipsProduct.Id
            };

            // Seed CachedProduct data
            modelBuilder.Entity<CachedProduct>().HasData(
                chocolateProduct,
                cookiesProduct,
                chipsProduct
            );

            // Seed StockItem data
            modelBuilder.Entity<StockItem>().HasData(
                chocolateStockItem,
                cookiesStockItem,
                chipsStockItem
            );
            
            modelBuilder.Entity<CachedProduct>()
                .HasOne(cp => cp.StockItem)
                .WithOne(si => si.CachedProduct)
                .HasForeignKey<StockItem>(si => si.CachedProductId);
        }
    }
}

