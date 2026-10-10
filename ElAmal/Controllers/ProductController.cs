using ElAmal.Core.Contract.Services.Products;
using ElAmal.Core.Helper;
using ElAmal.Core.Specifications.ProductSpecs;
using ElAmal.Shared.DTOs.Product;
using ElAmal.Shared.Errors;
using Microsoft.AspNetCore.Mvc;

namespace ElAmal.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
           _productServices = productServices;
        }

        [ProducesResponseType(typeof(Pagenation<ProductToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<Pagenation<ProductToReturnDto>>> GetAllAsync([FromQuery] ProductSpecParams specParams)
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
        public async Task<ActionResult<ProductToReturnDto>> GetEntityAsync(string? categorySlug, string? productSlug)
        {
            if (string.IsNullOrWhiteSpace(categorySlug) || string.IsNullOrWhiteSpace(productSlug))
                return BadRequest(new ApiResponse(404, "Category slug and product slug are required"));

            var product = await _productServices.GetEntityAsync(categorySlug, productSlug);

                if (product is null)
                    return NotFound(new ApiResponse(404, "Product Not Found"));

            return Ok(product);
        }
    }
}
