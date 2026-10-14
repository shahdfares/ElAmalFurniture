using ElAmal.Core.Helper;
using ElAmal.Core.Specifications.ProductSpecs;
using ElAmal.Shared.DTOs.Product;

namespace ElAmal.Core.Contract.Services.Products
{
    public interface IProductServices
    {
        Task<Pagenation<ProductToReturnDto>> GetAllAsync(ProductSpecParams spec);
        Task<ProductToReturnDto?> GetEntityAsync(string categorySlug, string productSlug);
        Task<ReturnResult<ProductToReturnDto>> AddAsync(ProductRequestDto productRequestDto);
        Task<ReturnResult<ProductToReturnDto>> UpdateAsync(Guid id, ProductRequestDto dto);
        Task<ReturnResult<bool>> DeleteAsync(Guid id);
        Task<ReturnResult<bool>> DeleteImageAsync(Guid productId, Guid imageId);
        Task<ReturnResult<bool>> SetPrimaryImageAsync(Guid productId, Guid imageId);

    }
}
