using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Entities.Products.TechnicalSpecifications
{
    public class SpecificationProperty : BaseEntity<Guid>
    {
        public string? Name { get; set; }

        public virtual ICollection<ProductTechnicalSpecification> ProductTechnicalSpecifications { get; set; }
             = new HashSet<ProductTechnicalSpecification>();

       
    }
}
