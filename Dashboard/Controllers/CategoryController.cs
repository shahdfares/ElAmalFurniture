using CloudinaryDotNet.Actions;
using ElAmal.Core.Contract.Services.Category;
using ElAmal.Core.Helper;
using ElAmal.Shared.DTOs.Category;
using ElAmal.Shared.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
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
        public async Task<ActionResult<CategoryResponseDto>> GetAll()
        {
            var categories = await _categoryServices.GetAllAsync();

            if(categories is null)
                return NotFound(new ApiResponse(404, "Not Categories Founded"));

            return Ok(categories);
        }

        [ProducesResponseType(typeof(ActionResult<CategoryResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> Add([FromBody]CategoryRequestDto categoryDto)
        {
            if (categoryDto is null)
                return BadRequest(new ApiResponse(400, "Invalid Request"));

            var addedCategory = await _categoryServices.AddAsync(categoryDto);

            if (!addedCategory.IsSuccess)
                return BadRequest(addedCategory);

            return Ok(addedCategory);
        }

        [ProducesResponseType(typeof(ActionResult<CategoryResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ReturnResult<CategoryResponseDto>>> Update(Guid id,CategoryRequestDto categoryDto)
        {
            if (categoryDto is null)
                return BadRequest(new ApiResponse(404, "Category name is required"));

            var updatedCategory = await _categoryServices.UpdateAsync(id, categoryDto);

         
             if (updatedCategory is null)
                 return NotFound(new ApiResponse(404, "No Category Found"));


            return Ok(updatedCategory);

        }

        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CategoryResponseDto>> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ApiResponse(400, "Invlid Request"));

            var deletedCategory = await _categoryServices.DeleteAsync(id);

            if (deletedCategory is null)
                return NotFound(new ApiResponse (404, "Category not found"));

            return Ok(deletedCategory);
        }
    }
}
