using ElAmal.Core.Contract.Services.SpecProperty;
using ElAmal.Core.Helper;
using ElAmal.Shared.DTOs.TechnicalSpecification;
using ElAmal.Shared.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    
    public class TechnicalSpecificationsController (ISpecificatonPropertyServices _propertyServices): BaseController
    {
        [ProducesResponseType(typeof(ReturnResult<IReadOnlyList<TechniacalSpecResponseDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<ReturnResult<IReadOnlyList<TechniacalSpecResponseDto>>>> GetAll()
        {
            var techProp = await _propertyServices.GetAllAsync();

            if (techProp is null)
                return NotFound(new ApiResponse(404, "No Technical Specification Founded"));

            return Ok(techProp);
        }

        [ProducesResponseType(typeof(ReturnResult<TechniacalSpecResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpPost]
        public async Task<ActionResult<ReturnResult<TechniacalSpecResponseDto>>> Add([FromBody] TechniacalSpecRequestDto requestDto)
        {
            if(requestDto is null)
                return BadRequest(new ApiResponse(400, "InvalidRequest"));

            var techProp = await _propertyServices.AddAsync(requestDto);

            if(techProp is null)
                return  NotFound(new ApiResponse(404, "No Technical Property Found"));

            return Ok(techProp);

        }

        [ProducesResponseType(typeof(ReturnResult<TechniacalSpecResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ReturnResult<TechniacalSpecResponseDto>>> Update(Guid id,  TechniacalSpecRequestDto requestDto)
        {
            if (id == Guid.Empty || requestDto is null)
                return BadRequest("Invalid request.");

            var techProp = await _propertyServices.UpdateAsync(id, requestDto);

            if (techProp is null)
                return NotFound(new ApiResponse(404, "No Technical Property Found"));

            return Ok(techProp);

        }

        [ProducesResponseType(typeof(ReturnResult<TechniacalSpecResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ReturnResult<TechniacalSpecResponseDto>>> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ApiResponse(400, "InvalidRequest"));

            var techProp = await _propertyServices.DeleteAsync(id);

            if (techProp is null)
                return NotFound(new ApiResponse(404, "No Technical Property Found"));

            return Ok(techProp);

        }
    }
}
