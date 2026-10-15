using ElAmal.Core.Entities.Products;
using Microsoft.EntityFrameworkCore;

namespace ElAmal.Core.Specifications.ProductSpecs
{
    public class ProductWithCategorySpecification : BaseSpecification<Product, Guid>
    {
        public ProductWithCategorySpecification(ProductSpecParams specParams) :
            base(P => (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search)) &&
                        string.IsNullOrEmpty(specParams.CategorySlug) || P.Category!.Slug == specParams.CategorySlug)
        {
            ApplyInclude();

            //Sort 
            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch(specParams.Sort) {

                    case "NameAsc": 
                        AddOrderBy(P => P.Name);
                        break;

                    case "NameDesc":
                        AddOrderByDesc(P => P.Name);
                        break;

                    case "Newest":
                        AddOrderByDesc(P => P.CreatedAt);
                        break;

                    case "Oldest":
                        AddOrderBy(P => P.CreatedAt);
                        break;

                    default:
                        AddOrderByDesc(P => P.CreatedAt);
                        break;

                }
            }

            // Pagenation
            ApplyPagenation((specParams.PageIndex - 1) * specParams.pageSize, specParams.pageSize);
        }

        public ProductWithCategorySpecification(string categorySlug, string productSlug) :
            base(P => P.Slug == productSlug && P.Category!.Slug == categorySlug)
        {
            ApplyInclude();
        }

        public ProductWithCategorySpecification(Guid? id) : base(p => p.Id == id)
        {
            ApplyInclude();
        }
        private void ApplyInclude()
        {
            Include.Add(query => query
                      .Include(c => c.Category));

            Include.Add(query => query
                      .Include(c => c.ProductImages));

            Include.Add(query => query
                      .Include(c => c.ProductTechnicalSpecifications!)
                      .ThenInclude( c => c.SpecificationProperties));
        }

    }
}
