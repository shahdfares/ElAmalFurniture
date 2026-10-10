using ElAmal.Core.Entities.Products.Images;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ElAmal.Infrastructure.Data.Configurations.Image
{
    internal class ProductImagesConfigurations : BaseEntityConfigurations<ProductImages, Guid>
    {
        public override void Configure(EntityTypeBuilder<ProductImages> builder)
        {
            base.Configure(builder);

            builder.Property(I => I.IsPrimary)
              .HasDefaultValue(false);

            builder.Property(I => I.DisplayOrder)
                   .HasDefaultValue(0);

        }
    
    }
}
