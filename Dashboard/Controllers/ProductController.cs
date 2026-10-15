using ElAmal.Core.Contract.Services.Products;
using ElAmal.Core.Helper;
using ElAmal.Core.Specifications.ProductSpecs;
using ElAmal.Shared.DTOs.Product;
using ElAmal.Shared.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    
    public class ProductController (IProductServices _productServices): BaseController
    {
        [ProducesResponseType(typeof(Pagenation<ProductToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<Pagenation<ProductToReturnDto>>> GetAll([FromQuery] ProductSpecParams specParams)
        {
            var products = await _productServices.GetAllAsync(specParams);

            if (products is null)
                return NotFound(new ApiResponse(404, "No Products Found"));

            return Ok(products);
        }

        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{categorySlug}/{productSlug}")]
        public async Task<ActionResult<ProductToReturnDto>> GetEntity(string? categorySlug, string? productSlug)
        {
            if (string.IsNullOrWhiteSpace(categorySlug) || string.IsNullOrWhiteSpace(productSlug))
                return BadRequest(new ApiResponse(404, "Category slug and product slug are required"));

            var product = await _productServices.GetEntityAsync(categorySlug, productSlug);

            if (product is null)
                return NotFound(new ApiResponse(404, "Product Not Found"));

            return Ok(product);
        }

        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [Consumes("multipart/form-data")]
        [HttpPost]
        public async Task<ActionResult<ProductToReturnDto>> Add([FromForm] ProductRequestDto productRequestDto)
        {
            if (productRequestDto is null)
                return BadRequest(new ApiResponse(404, "Invalid request"));

            var product = await _productServices.AddAsync(productRequestDto);

            if (!product.IsSuccess)
                return BadRequest(product);

            return Ok(product);
        }

        [HttpPut("{id:guid}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductToReturnDto>> Update(Guid id, [FromForm] ProductRequestDto dto)
        {
            var result = await _productServices.UpdateAsync(id, dto);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("{productId:guid}/primary-image/{imageId:guid}")]
        public async Task<ActionResult<bool>> SetPrimaryImage(Guid productId, Guid imageId)
        {
            if (productId == Guid.Empty && imageId == Guid.Empty)
                return BadRequest(new ApiResponse(400, "Invalid product id Or image id."));

            var result = await _productServices.SetPrimaryImageAsync(productId, imageId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new ApiResponse(400, "Invalid product id"));

            var result = await _productServices.DeleteAsync(id);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete("{productId:guid}/images/{imageId:guid}")]
        public async Task<ActionResult<bool>> DeleteImage(Guid productId, Guid imageId)
        {
            if (productId == Guid.Empty && imageId == Guid.Empty)
                return BadRequest(new ApiResponse(400, "Invalid product id Or image id."));

            var result = await _productServices.DeleteImageAsync(productId, imageId);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
        
    }
}

