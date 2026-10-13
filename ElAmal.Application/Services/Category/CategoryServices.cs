using AutoMapper;
using ElAmal.Core.Contract.Repository;
using ElAmal.Core.Contract.Services.Attachment;
using ElAmal.Core.Contract.Services.Category;
using ElAmal.Core.Entities.Products.Category;
using ElAmal.Core.Helper;
using ElAmal.Core.Specifications.CategorySpecs;
using ElAmal.Shared.DTOs.Category;

namespace ElAmal.Application.Services.Category
{
    public class CategoryServices(IUnitOfWork _unitOfWork, IMapper _mapper, IAttachmentServices _attachmentServices) : ICategoryServices
    {
       
        public async Task<IReadOnlyList<CategoryResponseDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.GetRepository<ProductCategory, Guid>().GetAllAsync();

            if (categories is null)
                return null!;

            return _mapper.Map<IReadOnlyList<CategoryResponseDto>>(categories);
        }

        public async Task<ReturnResult<CategoryResponseDto>> AddAsync(CategoryRequestDto categoryRequestDto)
        {
            if (categoryRequestDto is null)
                return ReturnResult<CategoryResponseDto>.Fail("Invalid Request");

            if (string.IsNullOrWhiteSpace(categoryRequestDto.Name))
                return ReturnResult<CategoryResponseDto>.Fail("Category Name is Required");

            categoryRequestDto.Name = categoryRequestDto.Name.Trim();


            //Generate Slug
            var slug = GenerateSlug.Generate(categoryRequestDto.Name);

            //Check if the name and slug is exist before 
            var checkOnNameAndSlug = await _unitOfWork.GetRepository<ProductCategory, Guid>().
                ExistsAsync(n => n.Name == categoryRequestDto.Name &&
                            n.Slug == slug);

            if(checkOnNameAndSlug)
                return ReturnResult<CategoryResponseDto>.Fail("Name and Slug are alraedy exsit");

            //Mapping 
            var category = _mapper.Map<ProductCategory>(categoryRequestDto);

            category.Slug = slug;

            //Add
            await _unitOfWork.GetRepository<ProductCategory, Guid>().AddAsync(category);

            //Save
            await _unitOfWork.CompleteAsync();


            return ReturnResult<CategoryResponseDto>.Success(
                _mapper.Map<CategoryResponseDto>(category),
                "Category Added Successfully");

        }

        public async Task<ReturnResult<CategoryResponseDto>> UpdateAsync(Guid id , CategoryRequestDto categoryDto)
        {
            //Vlaidation
            if (categoryDto is null && id == Guid.Empty)
                return ReturnResult<CategoryResponseDto>.Fail("Invalid Request");

            if (string.IsNullOrWhiteSpace(categoryDto!.Name))
                return ReturnResult<CategoryResponseDto>.Fail("Name is requird");

            //Clean
            categoryDto.Name = categoryDto.Name.Trim();

            //Generate Slug
            var slug = GenerateSlug.Generate(categoryDto.Name);

            //Check on Name and Slug
            var checkOnNameAndSlug = await _unitOfWork.GetRepository<ProductCategory, Guid>()
                .ExistsAsync(c => c.Name == categoryDto.Name &&
                             c.Slug == slug);

            if (checkOnNameAndSlug)
                return ReturnResult<CategoryResponseDto>.Fail("Name and Slug are exsit");

            var category = await _unitOfWork.GetRepository<ProductCategory, Guid>().GetEntityAync(id);

            if(category is null)
                return ReturnResult<CategoryResponseDto>.Fail("Category not found");

            category.Name = categoryDto.Name;
            category.Slug = slug;

            //Update 
             _unitOfWork.GetRepository<ProductCategory, Guid>().Update(category);

            //Save Changes
             await _unitOfWork.CompleteAsync();

            return ReturnResult<CategoryResponseDto>.Success(
                _mapper.Map<CategoryResponseDto>(category), "Category Updated Successfully");
        }

        public async Task<ReturnResult<CategoryResponseDto>> DeleteAsync(Guid? id)
        {
            if (id == Guid.Empty)
                return ReturnResult<CategoryResponseDto>.Fail(message: "Invalid category id.");

            //Check if exsits
            var exsit = await _unitOfWork.GetRepository<ProductCategory, Guid>()
                .ExistsAsync(x => x.Id == id);

            if (!exsit) //True 
                return ReturnResult<CategoryResponseDto>.Fail("Category not found");

            var spec = new CategoryWithProductsAndImages(id!.Value);

            var category = await _unitOfWork.GetRepository<ProductCategory, Guid>().GetEntityWithSpecAync(spec);

           //Delete product's images
           foreach(var product in category!.Products)
            {
                if (product.ProductImages is null)
                    continue;

                foreach(var images in product.ProductImages)
                {
                   await _attachmentServices.DeleteAsync(images.PublicId);
                }
            }

            _unitOfWork.GetRepository<ProductCategory, Guid>().Delete(category);

            return ReturnResult<CategoryResponseDto>.Success(
                _mapper.Map<CategoryResponseDto>(category),
                "Category Deleted Successfully");
        }
    }
}
