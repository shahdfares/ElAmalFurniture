using ElAmal.Core.Helper;
using ElAmal.Core.Specifications.ProductSpecs;
using ElAmal.Shared.DTOs.Product;

namespace ElAmal.Core.Contract.Services.Products
{
    public interface IProductServices
    {
        Task<Pagenation<ProductToReturnDto>> GetAllAsync(ProductSpecParams spec);
        Task<ProductToReturnDto?> GetEntityAsync(string categorySlug, string productSlug);
    }
}
