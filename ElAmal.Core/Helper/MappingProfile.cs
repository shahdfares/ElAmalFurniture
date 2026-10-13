using AutoMapper;
using ElAmal.Core.Entities.Products;
using ElAmal.Core.Entities.Products.Category;
using ElAmal.Core.Entities.Products.Images;
using ElAmal.Core.Entities.Products.TechnicalSpecifications;
using ElAmal.Shared.DTOs.Category;
using ElAmal.Shared.DTOs.Image;
using ElAmal.Shared.DTOs.Product;
using ElAmal.Shared.DTOs.TechnicalSpecification;

namespace ElAmal.Core.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Category!.Name))
                .ForMember(d => d.CategorySlug, o => o.MapFrom(s => s.Category!.Slug));

            CreateMap<ProductImages, ImageResponseDto>();

            CreateMap<ProductTechnicalSpecification, TechnicalSpecificationResponse>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.SpecificationProperties!.Name));

            CreateMap<ProductCategory, CategoryResponseDto>();

            CreateMap<CategoryRequestDto, ProductCategory>();
            
            CreateMap<SpecificationProperty, TechniacalSpecResponseDto>();

            CreateMap<TechniacalSpecResponseDto, SpecificationProperty>();


        }
    }
}
