using ElAmal.Core.Entities.Products.TechnicalSpecifications;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Infrastructure.Data.Configurations.TechnicalSpecifications
{
    internal class SpecificationPropertyConfigurations : BaseEntityConfigurations<SpecificationProperty, Guid>
    {
        public override void Configure(EntityTypeBuilder<SpecificationProperty> builder)
        {
            base.Configure(builder);

            builder.Property(M => M.Name).IsRequired();

            builder.HasIndex(M => M.Name)
               .IsUnique();
        }
    }
}
