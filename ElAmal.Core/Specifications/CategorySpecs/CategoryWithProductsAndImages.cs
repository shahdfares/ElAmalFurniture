using ElAmal.Core.Entities.Products.Category;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Specifications.CategorySpecs
{
    public class CategoryWithProductsAndImages : BaseSpecification<ProductCategory, Guid>
    {
        public CategoryWithProductsAndImages(Guid categoryId)
            : base(c => c.Id == categoryId )
        {
            ApplyInclude();
        }

        private void ApplyInclude()
        {
            Include.Add(query => query
                     .Include(c => c.Products)
                     .ThenInclude(c => c.ProductImages));

        }
    }
}
