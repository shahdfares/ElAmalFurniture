using ElAmal.Core.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Infrastructure.Data.Configurations.Products
{
    internal class ProductConfigurations : BaseAuditableConfigurations<Product, Guid>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.Property(P => P.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(P => P.Description)
                .HasMaxLength(700);

            builder.Property(P => P.Code)
                .HasMaxLength(100);


            builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Slug).IsRequired().HasMaxLength(200);
            
            builder.HasIndex(x => x.Slug).IsUnique();
        }

        }
    }
