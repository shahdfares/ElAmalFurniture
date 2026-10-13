using ElAmal.Core.Helper;
using ElAmal.Shared.DTOs.TechnicalSpecification;

namespace ElAmal.Core.Contract.Services.SpecProperty
{
    public interface ISpecificatonPropertyServices
    {
        Task<ReturnResult<IReadOnlyList<TechniacalSpecResponseDto>>> GetAllAsync();
        Task<ReturnResult<TechniacalSpecResponseDto>> AddAsync(TechniacalSpecRequestDto responseDto);
        Task<ReturnResult<TechniacalSpecResponseDto>> UpdateAsync(Guid id, TechniacalSpecRequestDto responseDto);
        Task<ReturnResult<TechniacalSpecResponseDto>> DeleteAsync(Guid id);
    }
}
