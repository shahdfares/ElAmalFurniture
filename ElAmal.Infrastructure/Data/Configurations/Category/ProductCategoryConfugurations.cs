using ElAmal.Core.Entities.Products.Category;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Infrastructure.Data.Configurations.Category
{
    internal class ProductCategoryConfugurations : BaseAuditableConfigurations<ProductCategory, Guid>
    {
        public override void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            base.Configure(builder);

            builder.Property(C => C.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(c => c.Name).IsUnique();

            builder.Property(x => x.Slug).IsRequired().HasMaxLength(200);

            builder.HasIndex(c => c.Slug).IsUnique();
        }
    }
}
