﻿
using Domain.StoreSystem.models;
using Microsoft.EntityFrameworkCore;

namespace Store.Integration
{
    public class StoreContext : DbContext
    {
        
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }
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
            var storeId = Environment.GetEnvironmentVariable("STORE_ID");

            var chocolateProduct = new CachedProduct()
                { Id = 1001L, Name = "Chocolate", Barcode = "Chocolate", CurrentPrice = 1.50, ProductId = 1L, SupplierId = 1L};
            var cookiesProduct = new CachedProduct
                { Id = 1002L, Name = "Cookies", Barcode = "Cookies", CurrentPrice = 2.00, ProductId = 2L, SupplierId = chocolateProduct.SupplierId};
            var chipsProduct = new CachedProduct
                { Id = 1003L, Name = "Chips", Barcode = "Chips", CurrentPrice = 1.00, ProductId = 3L, SupplierId = 2L};
            
            var bananaProduct = new CachedProduct
                { Id = 1004L, Name = "Banana", Barcode = "Banana", CurrentPrice = 0.50, ProductId = 4L, SupplierId = 3L};
            
            var strawberryProduct = new CachedProduct
                { Id = 1005L, Name = "Strawberry", Barcode = "Strawberry", CurrentPrice = 0.75, ProductId = 5L, SupplierId = bananaProduct.SupplierId};

            if (storeId == "1001")
            {
                modelBuilder.Entity<StockItem>().HasData(
                    new StockItem()
                    {
                        Id = 2001L, Name = "Chocolate", AvailableQuantity = 41,
                        CachedProductId = chocolateProduct.Id, OutGoingQuantity = 0, MinStock = 40
                    },
                    new StockItem()
                    {
                        Id = 2002L, Name = "Cookies", AvailableQuantity = 100,
                        CachedProductId = cookiesProduct.Id, OutGoingQuantity = 0, MinStock = 40
                    },
                    new StockItem()
                    {
                        Id = 2003L, Name = "Chips", AvailableQuantity = 110,
                        CachedProductId = chipsProduct.Id, OutGoingQuantity = 0, MinStock = 40
                    },
                    new StockItem()
                    {
                        Id = 2004L, Name = "Banana", AvailableQuantity = 20,
                        CachedProductId = bananaProduct.Id, OutGoingQuantity = 0, MinStock = 40
                    },
                    new StockItem()
                    {
                        Id = 2005L, Name = "Strawberry", AvailableQuantity = 40,
                        CachedProductId = strawberryProduct.Id, OutGoingQuantity = 0, MinStock = 40
                    }
                );
            }
            else if (storeId == "1002")
            {
                modelBuilder.Entity<StockItem>().HasData(
                    new StockItem()
                    {
                        Id = 2001L, Name = "Chocolate", AvailableQuantity = 130,
                        CachedProductId = chocolateProduct.Id, OutGoingQuantity = 0, MinStock = 40
                    },
                    new StockItem()
                    {
                        Id = 2002L, Name = "Cookies", AvailableQuantity = 40,
                        CachedProductId = cookiesProduct.Id, OutGoingQuantity = 0, MinStock = 40
                    },
                    new StockItem()
                    {
                        Id = 2003L, Name = "Chips", AvailableQuantity = 50,
                        CachedProductId = chipsProduct.Id, OutGoingQuantity = 0, MinStock = 40
                    },
                    new StockItem()
                    {
                        Id = 2004L, Name = "Banana", AvailableQuantity = 43,
                        CachedProductId = bananaProduct.Id, OutGoingQuantity = 0, MinStock = 40
                    },
                    new StockItem()
                    {
                        Id = 2005L, Name = "Strawberry", AvailableQuantity = 120,
                        CachedProductId = strawberryProduct.Id, OutGoingQuantity = 0, MinStock = 40
                    }
                );
            }

            // Seed CachedProduct data
            modelBuilder.Entity<CachedProduct>().HasData(
                chocolateProduct,
                cookiesProduct,
                chipsProduct,
                bananaProduct,
                strawberryProduct
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

