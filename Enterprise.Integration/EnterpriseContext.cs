using Domain.Enterprise.models;
using Microsoft.EntityFrameworkCore;

namespace Enterprise.Integration;

public class EnterpriseContext : DbContext
{
    public EnterpriseContext(DbContextOptions<EnterpriseContext> options) : base(options)
    {
    }
    public DbSet<Domain.Enterprise.Models.Enterprise> Enterprises { get; set; }
    
    public DbSet<DeliveryLog> DeliveryLogs { get; set; }
    
    public DbSet<Product> Products { get; set; }
    
    public DbSet<Supplier> Suppliers { get; set; }
    
    public DbSet<EnterpriseSupplier> EnterpriseSuppliers { get; set; }
    
    public DbSet<StoreMetaData> StoreMetaDatas { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("EnterpriseSystem");
    }
}