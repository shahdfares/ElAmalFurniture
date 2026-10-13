using ElAmal.Core.Helper;
using ElAmal.Shared.DTOs.Category;

namespace ElAmal.Core.Contract.Services.Category
{
    public interface ICategoryServices
    {
        Task<IReadOnlyList<CategoryResponseDto>> GetAllAsync();

        // For Dashboard
        Task<ReturnResult<CategoryResponseDto>> AddAsync(CategoryRequestDto categoryRequestDto);
        Task<ReturnResult<CategoryResponseDto>> UpdateAsync(CategoryRequestDto categoryDto);
        Task<ReturnResult<CategoryResponseDto>> DeleteAsync(Guid? Id);
    }
}
