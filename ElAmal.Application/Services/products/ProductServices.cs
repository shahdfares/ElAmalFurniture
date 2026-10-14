using AutoMapper;
using ElAmal.Core.Contract.Repository;
using ElAmal.Core.Contract.Services.Attachment;
using ElAmal.Core.Contract.Services.Products;
using ElAmal.Core.Entities.Products;
using ElAmal.Core.Entities.Products.Category;
using ElAmal.Core.Entities.Products.Images;
using ElAmal.Core.Entities.Products.TechnicalSpecifications;
using ElAmal.Core.Helper;
using ElAmal.Core.Specifications.ProductSpecs;
using ElAmal.Shared.DTOs.Attachment;
using ElAmal.Shared.DTOs.Product;

namespace ElAmal.Application.Services.products
{
    public class ProductServices : IProductServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentServices _attachmentServices;

        public ProductServices(IUnitOfWork unitOfWork,
            IMapper mapper,
            IAttachmentServices attachmentServices)
        {
           _unitOfWork = unitOfWork;
           _mapper = mapper;
            _attachmentServices = attachmentServices;
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

        public async Task<ReturnResult<ProductToReturnDto>> AddAsync(ProductRequestDto productRequestDto)
        {
            #region Validation

            if (productRequestDto is null)
                return ReturnResult<ProductToReturnDto>.Fail("Invalid request");

            if (string.IsNullOrWhiteSpace(productRequestDto.Name))
                return ReturnResult<ProductToReturnDto>.Fail("Name is required");

            productRequestDto.Name = productRequestDto.Name.Trim();

            if (!productRequestDto.CategoryId.HasValue || productRequestDto.CategoryId == Guid.Empty)
                return ReturnResult<ProductToReturnDto>.Fail("Category is required.");

            if (productRequestDto.Images is null || !productRequestDto.Images.Any())
                return ReturnResult<ProductToReturnDto>.Fail("At least one product image is required.");

            #endregion

            #region Check Category

            var category = await _unitOfWork.GetRepository<ProductCategory, Guid>().GetEntityAync(productRequestDto.CategoryId.Value);

            if (category is null)
                return ReturnResult<ProductToReturnDto>.Fail("Categpry not found.");

            #endregion

            
            #region Check Name Duplication

            var slug = GenerateSlug.Generate(productRequestDto.Name);

            var check = await _unitOfWork.GetRepository<Product, Guid>()
                .ExistsAsync(p => p.Name.ToLower() == productRequestDto.Name.ToLower() && p.Slug == slug);

            if (check) // True
                return ReturnResult<ProductToReturnDto>.Fail("Name and slug already exsits");

            #endregion

            #region Technical Property Validation

            //Aviod duplication
            var specificationIds = productRequestDto.technicalSpecificationRequests
                .Select( t=> t.TechnicalSpecificationId)
                .ToList();

            if(specificationIds.Distinct().Count() != specificationIds.Count)
                return ReturnResult<ProductToReturnDto>.Fail("Technical specifications cannot be duplicated.");

            //Check if Tech Prop Exsits Or not
            foreach ( var item in productRequestDto.technicalSpecificationRequests)
              {
                  var techprop = await _unitOfWork.GetRepository<SpecificationProperty, Guid>()
                      .ExistsAsync(p => p.Id == item.TechnicalSpecificationId);
            
                  if (!techprop)
                      return ReturnResult<ProductToReturnDto>.Fail("Technical specification does not exist");
              }

            #endregion

            #region Mapping

            var product = _mapper.Map<Product>(productRequestDto);

            product.Slug = slug;

            #endregion

            #region Upload Images

            var uploadedImages = new List<UploadResultDto>();

            try
            {
                for (var i = 0; i < productRequestDto.Images.Count; i++)
                {
                    var uploadedImage = await _attachmentServices.UploadAsync(productRequestDto.Images[i], "Products");

                    if (uploadedImage is null)
                    {
                        foreach (var image in uploadedImages)
                            await _attachmentServices.DeleteAsync(image.PublicId);

                        return ReturnResult<ProductToReturnDto>.Fail("Failed to upload product image.");
                    }

                    uploadedImages.Add(uploadedImage);

                    product.ProductImages!.Add(new ProductImages
                    {
                        ImagesUrl = uploadedImage.ImageUrl,
                        PublicId = uploadedImage.PublicId,
                        IsPrimary = i == 0,
                        DisplayOrder = i
                    });
                }

                #region Save Product

                await _unitOfWork.GetRepository<Product, Guid>()
                    .AddAsync(product);

                await _unitOfWork.CompleteAsync();

                #endregion
            }
            catch (Exception)
            {
                #region Delete Uploaded Images

                foreach (var image in uploadedImages)
                    await _attachmentServices.DeleteAsync(image.PublicId);

                #endregion

               return ReturnResult<ProductToReturnDto>.Fail("Failed to create product.");

               // return ReturnResult<ProductToReturnDto>.Fail(ex.ToString());
            }

            #endregion

            var response = _mapper.Map<ProductToReturnDto>(product);

            return ReturnResult<ProductToReturnDto>.Success(response, "Product added successfully.");
        }

        public async Task<ReturnResult<ProductToReturnDto>> UpdateAsync(Guid id, ProductRequestDto productRequestDto)
        {
            #region Validation

            if (productRequestDto is null)
                return ReturnResult<ProductToReturnDto>.Fail("Invalid request");

            if (string.IsNullOrWhiteSpace(productRequestDto.Name))
                return ReturnResult<ProductToReturnDto>.Fail("Name is required");

            productRequestDto.Name = productRequestDto.Name.Trim();

            if (!productRequestDto.CategoryId.HasValue || productRequestDto.CategoryId == Guid.Empty)
                return ReturnResult<ProductToReturnDto>.Fail("Category is required.");

            if (productRequestDto.Images is null || !productRequestDto.Images.Any())
                return ReturnResult<ProductToReturnDto>.Fail("At least one product image is required.");

            #endregion

            #region Get Product

            var spec = new ProductWithCategorySpecification(id);

            var product = await _unitOfWork
                .GetRepository<Product, Guid>()
                .GetEntityWithSpecAync(spec);

            if (product is null)
                return ReturnResult<ProductToReturnDto>.Fail("Product not found.");

            if (product.CategoryId != productRequestDto.CategoryId)
                return ReturnResult<ProductToReturnDto>.Fail("CategoryId does not match");

            #endregion

            #region Check Duplicate Name

            var existingProduct = await _unitOfWork
                .GetRepository<Product, Guid>()
                .ExistsAsync(p => p.Name.ToLower() == productRequestDto.Name.ToLower());

            if (existingProduct)
                return ReturnResult<ProductToReturnDto>.Fail("Name already exsits");

            #endregion

            #region Update Specifiaction Property

            var existingSpecifications = product.ProductTechnicalSpecifications!
                .ToDictionary(p => p.SpecificationPropertiesId);

            foreach (var item in productRequestDto.technicalSpecificationRequests)
            {
                if (existingSpecifications.TryGetValue(item.TechnicalSpecificationId, out var exsiting))
                {
                    //Update
                    exsiting.Value = item.Value;
                }

                else
                {
                    //Add new 
                    var newSpecification = new ProductTechnicalSpecification()
                    {
                        ProductId = product.Id,
                        SpecificationPropertiesId = item.TechnicalSpecificationId,
                        Value = item.Value

                    };

                    product.ProductTechnicalSpecifications!.Add(newSpecification);

                    existingSpecifications.Add(item.TechnicalSpecificationId, newSpecification);
                }
            }
            #endregion

            #region Upload New Images

            var uploadedImages = new List<UploadResultDto>();

            try
            {
                if (productRequestDto.Images is not null && productRequestDto.Images.Any())
                {
                    foreach (var image in productRequestDto.Images)
                    {
                        var uploadedImage = await _attachmentServices
                            .UploadAsync(image, "Products");

                        if (uploadedImage is null)
                        {
                            foreach (var uploaded in uploadedImages)
                                await _attachmentServices.DeleteAsync(uploaded.PublicId);

                            return ReturnResult<ProductToReturnDto>
                                .Fail("Failed to upload product image.");
                        }

                        uploadedImages.Add(uploadedImage);

                        var displayOrder = product.ProductImages!.Any()
                            ? product.ProductImages!.Max(x => x.DisplayOrder) + 1
                            : 0;

                        product.ProductImages!.Add(new ProductImages
                        {
                            ImagesUrl = uploadedImage.ImageUrl,
                            PublicId = uploadedImage.PublicId,
                            DisplayOrder = displayOrder,
                            IsPrimary = !product.ProductImages.Any(x => x.IsPrimary)
                        });
                    }
                }

                #region Save Product

                _unitOfWork
                    .GetRepository<Product, Guid>()
                    .Update(product);

                await _unitOfWork.CompleteAsync();
            }
                #endregion

            catch
            {
                foreach (var uploaded in uploadedImages)
                    await _attachmentServices.DeleteAsync(uploaded.PublicId);

                return ReturnResult<ProductToReturnDto>
                    .Fail("Failed to update product.");
            }

            #endregion

            var response = _mapper.Map<ProductToReturnDto>(product);

            return ReturnResult<ProductToReturnDto>.Success(
                response,
                "Product updated successfully.");

        }

        public async Task<ReturnResult<bool>> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return ReturnResult<bool>.Fail("Invalid product id.");
          
            //Get Product
            var spec = new ProductWithImageSpecification(id);

            var product = await _unitOfWork
                .GetRepository<Product, Guid>()
                .GetEntityWithSpecAync(spec);

            if (product is null)
                return ReturnResult<bool>.Fail("Product not found.");

            //Delete Product 
            try
            {
                //Delete producr's iamges first
                if (product.ProductImages is not null && product.ProductImages.Any())
                {
                    foreach (var image in product.ProductImages)
                    {
                        await _attachmentServices.DeleteAsync(image.PublicId);
                    }
                }

                _unitOfWork.GetRepository<Product, Guid>().Delete(product);

                await _unitOfWork.CompleteAsync();

                return ReturnResult<bool>.Success(true, "Product deleted successfully.");
            }
            catch
            {
                return ReturnResult<bool>.Fail(
                    "Failed to delete product.");
            }


        }

        public async Task<ReturnResult<bool>> DeleteImageAsync(Guid productId, Guid imageId)
        {
            if (productId == Guid.Empty)
                return ReturnResult<bool>.Fail("Invalid product id.");

            if (imageId == Guid.Empty)
                return ReturnResult<bool>.Fail("Invalid image id.");

            #region Get Product With Images

            var spec = new ProductWithImageSpecification(productId);

            var product = await _unitOfWork
                .GetRepository<Product, Guid>()
                .GetEntityWithSpecAync(spec);

            if (product is null)
                return ReturnResult<bool>.Fail("Product not found.");

            #endregion


            #region Get Image

            var image = product.ProductImages?
                .FirstOrDefault(x => x.Id == imageId);

            if (image is null)
                return ReturnResult<bool>.Fail("Image not found.");

            #endregion


            try
            {
                #region Delete From Cloudinary

                if (!string.IsNullOrWhiteSpace(image.PublicId))
                {
                    await _attachmentServices.DeleteAsync(image.PublicId);
                }

                #endregion


                #region Remove Image From Product

                var wasPrimary = image.IsPrimary;

                product.ProductImages!.Remove(image);

                #endregion


                #region Reassign Primary Image If Needed

                if (wasPrimary && product.ProductImages!.Any())
                {
                    var newPrimary = product.ProductImages
                        .OrderBy(x => x.DisplayOrder)
                        .First();

                    newPrimary.IsPrimary = true;
                }

                #endregion


                #region Save

                _unitOfWork
                    .GetRepository<Product, Guid>()
                    .Update(product);

                await _unitOfWork.CompleteAsync();

                #endregion

                return ReturnResult<bool>.Success(true, "Image deleted successfully.");
            }
            catch
            {
                return ReturnResult<bool>.Fail("Failed to delete image.");
            }

        }

        public async Task<ReturnResult<bool>> SetPrimaryImageAsync(Guid productId, Guid imageId)
        {
            #region Validation

            if (productId == Guid.Empty)
                return ReturnResult<bool>.Fail("Invalid product id.");

            if (imageId == Guid.Empty)
                return ReturnResult<bool>.Fail("Invalid image id.");

            #endregion


            #region Get Product

            var product = await _unitOfWork
                .GetRepository<Product, Guid>()
                .GetEntityWithSpecAync(
                    new ProductWithImageSpecification(productId));

            if (product is null)
                return ReturnResult<bool>.Fail("Product not found.");

            #endregion


            #region Check Image

            var image = product.ProductImages?
                .FirstOrDefault(x => x.Id == imageId);

            if (image is null)
                return ReturnResult<bool>.Fail("Image not found.");

            #endregion


            #region Change Primary Image

            foreach (var productImage in product.ProductImages!)
            {
                productImage.IsPrimary = false;
            }


            image.IsPrimary = true;


            #endregion


            #region Save

            _unitOfWork
                .GetRepository<Product, Guid>()
                .Update(product);

            await _unitOfWork.CompleteAsync();

            #endregion


            return ReturnResult<bool>.Success(
                true,
                "Primary image updated successfully.");
        }
    }
}
