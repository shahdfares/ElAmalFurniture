using ElAmal.Core.Entities.Products;
using ElAmal.Core.Entities.Products.Category;
using ElAmal.Core.Entities.Products.Images;
using ElAmal.Core.Entities.Products.TechnicalSpecifications;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ElAmal.Infrastructure.Data
{
    public class ElAmalDbContext : DbContext
    {
        public ElAmalDbContext(DbContextOptions<ElAmalDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Entity<ProductTechnicalSpecification>().HasKey(k => new { k.ProductId, k.SpecificationPropertiesId });

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        #region DbSets

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> productCategories { get; set; }
       
        public DbSet<SpecificationProperty> SpecificationProperties { get; set; }
        public DbSet<ProductTechnicalSpecification> ProductTechnicalSpecifications { get; set; }
        public DbSet<ProductImages> ProductImages { get; set; }
      
        #endregion
    }
}
