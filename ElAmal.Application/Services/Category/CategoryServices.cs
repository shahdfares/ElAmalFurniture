using AutoMapper;
using ElAmal.Core.Contract.Repository;
using ElAmal.Core.Contract.Services.Category;
using ElAmal.Core.Entities.Products.Category;
using ElAmal.Shared.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Application.Services.Category
{
    public class CategoryServices(IUnitOfWork _unitOfWork, IMapper _mapper) : ICategoryServices
    {
        public async Task<IReadOnlyList<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.GetRepository<ProductCategory, Guid>().GetAllAsync();

            if (categories is null)
                return null!;

            return _mapper.Map<IReadOnlyList<CategoryResponseDto>>(categories);
        }
    }
}
