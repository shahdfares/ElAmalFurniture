using ElAmal.Core.Entities.Products;
using ElAmal.Core.Helper;
using ElAmal.Core.Specifications.ProductSpecs;
using ElAmal.Shared.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Contract.Services.Products
{
    public interface IProductServices
    {
        Task<Pagenation<ProductToReturnDto>> GetAllAsync(ProductSpecParams spec);
        Task<ProductToReturnDto?> GetEntityAsync(string categorySlug, string productSlug);
    }
}
