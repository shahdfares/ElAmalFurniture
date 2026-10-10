using ElAmal.Core.Entities.Products.TechnicalSpecifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Entities.Products.Category
{
    public class ProductCategory : BaseAuditable<Guid>
    {
        public required string Name { get; set; }
        public string? Slug { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
      
    }
}
