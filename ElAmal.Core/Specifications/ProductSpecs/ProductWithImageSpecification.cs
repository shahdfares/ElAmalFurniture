using ElAmal.Core.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Specifications.ProductSpecs
{
    public class ProductWithImageSpecification : BaseSpecification<Product, Guid>
    {
        public ProductWithImageSpecification(Guid id)
            : base(x => x.Id == id)
        {
            Include.Add(p => p.Include(p => p.ProductImages));
        }
    }
}
