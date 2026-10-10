using ElAmal.Core.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Specifications.ProductSpecs
{
    public class ProductWithFiltrationForCount : BaseSpecification<Product,Guid>
    {
        public ProductWithFiltrationForCount(ProductSpecParams specParams): 
            base(P =>  (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search)) &&
                        (string.IsNullOrEmpty(specParams.CategorySlug) || P.Category!.Slug == specParams.CategorySlug))
        {
            
        }
    }
}
