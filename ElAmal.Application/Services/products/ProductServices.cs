using AutoMapper;
using ElAmal.Core.Contract.Repository;
using ElAmal.Core.Contract.Services.Products;
using ElAmal.Core.Entities.Products;
using ElAmal.Core.Helper;
using ElAmal.Core.Specifications.ProductSpecs;
using ElAmal.Shared.DTOs.Product;

namespace ElAmal.Application.Services.products
{
    public class ProductServices : IProductServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductServices(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
           _unitOfWork = unitOfWork;
           _mapper = mapper;
        }

        public async Task<Pagenation<ProductToReturnDto>> GetAllAsync(ProductSpecParams spec)
        {
            var productSpec =  new ProductWithCategorySpecification(spec);

            var products = await _unitOfWork.GetRepository<Product, Guid>().GetAllWithSpecAsync(productSpec);

            var data =  _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            var count = await GetCount(spec);

            return (new Pagenation<ProductToReturnDto>(spec.PageIndex, spec.pageSize, count, data));
        }
 
        public async Task<ProductToReturnDto?> GetEntityAsync(string categorySlug, string productSlug)
        {
            if (string.IsNullOrWhiteSpace(categorySlug) || string.IsNullOrWhiteSpace(productSlug))
                return null;

            var spec = new ProductWithCategorySpecification(categorySlug,  productSlug);

            var products = await _unitOfWork.GetRepository<Product, Guid>().GetEntityWithSpecAync(spec);

            if (products is null)
                return null;

            return _mapper.Map<ProductToReturnDto>(products);

        }

        private async Task<int> GetCount(ProductSpecParams specParams)
        {
            var countSpec = new ProductWithFiltrationForCount(specParams);

            var count = await _unitOfWork.GetRepository<Product, Guid>().GetCount(countSpec);

            return count;
        }
    }
}
