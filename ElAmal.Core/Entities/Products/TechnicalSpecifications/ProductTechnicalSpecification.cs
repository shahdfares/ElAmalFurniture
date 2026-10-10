using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Entities.Products.TechnicalSpecifications
{
    public class ProductTechnicalSpecification : BaseEntity<Guid>
    {
        public string? Value { get; set; }

        // Navigational propety 
        public Guid SpecificationPropertiesId { get; set; }
        public SpecificationProperty? SpecificationProperties { get; set; }

        public Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
