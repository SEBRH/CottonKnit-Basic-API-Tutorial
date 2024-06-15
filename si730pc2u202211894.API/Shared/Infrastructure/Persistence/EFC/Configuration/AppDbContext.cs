using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using si730pc2u202211894.API.sale.Domain.Models.Aggregates;
using si730pc2u202211894.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

namespace si730pc2u202211894.API.Shared.Infrastructure.Persistence.EFC.Configuration; 

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
        // Enable Audit Fields Interceptors
        builder.AddCreatedUpdatedInterceptor();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // PurchaseOrder Context
        
        builder.Entity<PurchaseOrder>().HasKey(c => c.Id);
        builder.Entity<PurchaseOrder>().Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<PurchaseOrder>().Property(c => c.Customer).IsRequired().HasMaxLength(50);
        builder.Entity<PurchaseOrder>().Property(c => c.FabricId).IsRequired();
        builder.Entity<PurchaseOrder>().Property(c => c.City).IsRequired().HasMaxLength(50);
        builder.Entity<PurchaseOrder>().Property(c => c.ResumeUrl).IsRequired().HasMaxLength(50);
        builder.Entity<PurchaseOrder>().Property(c => c.Quantity).IsRequired();
        
        // Apply SnakeCase Naming Convention
        builder.UseSnakeCaseWithPluralizedTableNamingConvention();
    }
}