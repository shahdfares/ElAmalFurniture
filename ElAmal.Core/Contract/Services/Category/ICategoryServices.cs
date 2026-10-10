using ElAmal.Core.Entities.Products.Category;
using ElAmal.Shared.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Contract.Services.Category
{
    public interface ICategoryServices
    {
        Task<IReadOnlyList<CategoryResponseDto>> GetAllAsync();
    }
}
