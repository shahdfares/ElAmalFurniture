using ElAmal.Core.Entities.Products.Category;
using ElAmal.Core.Entities.Products.Images;
using ElAmal.Core.Entities.Products.TechnicalSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Entities.Products
{
    public class Product : BaseAuditable<Guid>
    {
        public required string Name { get; set; } 
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }

        // Navigational properties

        public Guid CategoryId { get; set; }
        public ProductCategory? Category { get; set; }

        public virtual ICollection<ProductImages>? ProductImages { get; set; } = new HashSet<ProductImages>();
        public virtual ICollection<ProductTechnicalSpecification>? ProductTechnicalSpecifications { get; set; } = new HashSet<ProductTechnicalSpecification>();

    }
}
