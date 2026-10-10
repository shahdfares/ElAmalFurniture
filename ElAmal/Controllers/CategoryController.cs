using ElAmal.Core.Contract.Services.Category;
using ElAmal.Shared.DTOs.Category;
using ElAmal.Shared.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElAmal.Controllers
{
    
    public class CategoryController : BaseController
    {
        private readonly ICategoryServices _categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        [ProducesResponseType(typeof(CategoryResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CategoryResponseDto>>> GetAllAsync()
        {
            var categories = await _categoryServices.GetAllAsync();

            if (categories is null)
                return NotFound(new ApiResponse(404, "No Products Found"));

            return Ok(categories);
        }
    }
}
