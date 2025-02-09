
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
                { Id = Guid.NewGuid(), Name = "Chocolate", Barcode = "Chocolate", CurrentPrice = 1.50, ProductId = Guid.NewGuid() };
            var cookiesProduct = new CachedProduct
                { Id = Guid.NewGuid(), Name = "Cookies", Barcode = "Cookies", CurrentPrice = 2.00, ProductId = Guid.NewGuid() };
            var chipsProduct = new CachedProduct
                { Id = Guid.NewGuid(), Name = "Chips", Barcode = "Chips", CurrentPrice = 1.00, ProductId = Guid.NewGuid() };

            var chocolateStockItem = new StockItem()
            {
                Id = Guid.NewGuid(), Name = "Chocolate", AvailableQuantity = 100,
                CachedProductId = chocolateProduct.Id, OutGoingQuantity = 0
            };
            var cookiesStockItem = new StockItem()
            {
                Id = Guid.NewGuid(), Name = "Cookies", AvailableQuantity = 100,
                CachedProductId = cookiesProduct.Id, OutGoingQuantity = 0
            };
            var chipsStockItem = new StockItem()
            {
                Id = Guid.NewGuid(), Name = "Chips", AvailableQuantity = 100,
                CachedProductId = chipsProduct.Id, OutGoingQuantity = 0
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
            
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderSupplier)
                .WithOne(os => os.Order)
                .HasForeignKey(os => os.OrderId);
         
            modelBuilder.Entity<OrderSupplierCachedProduct>()
                .HasKey(osp => new { osp.OrderSupplierId, osp.CachedProductId });

            modelBuilder.Entity<OrderSupplierCachedProduct>()
                .HasOne(osp => osp.OrderSupplier)
                .WithMany(os => os.OrderSupplierProducts)
                .HasForeignKey(osp => osp.OrderSupplierId);

            modelBuilder.Entity<OrderSupplierCachedProduct>()
                .HasOne(osp => osp.CachedProduct)
                .WithMany(cp => cp.OrderSupplierProducts)
                .HasForeignKey(osp => osp.CachedProductId);
            
        }
    }
}

