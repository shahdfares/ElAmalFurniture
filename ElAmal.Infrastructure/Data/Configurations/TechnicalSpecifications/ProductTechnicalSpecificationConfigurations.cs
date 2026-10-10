using ElAmal.Core.Entities.Products.TechnicalSpecifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElAmal.Infrastructure.Data.Configurations.TechnicalSpecifications
{
    internal class ProductTechnicalSpecificationConfigurations : BaseEntityConfigurations<ProductTechnicalSpecification, Guid>
    {
        public override void Configure(EntityTypeBuilder<ProductTechnicalSpecification> builder)
        {

            builder.HasOne(m => m.Product)
             .WithMany(p => p.ProductTechnicalSpecifications)
             .HasForeignKey(m => m.ProductId)
             .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.SpecificationProperties)
                .WithMany(p => p.ProductTechnicalSpecifications)
                .HasForeignKey( p => p.SpecificationPropertiesId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
