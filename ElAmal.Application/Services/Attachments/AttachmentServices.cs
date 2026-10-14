using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ElAmal.Core.Contract.Services.Attachment;
using ElAmal.Core.Entities.Products.Attachments;
using ElAmal.Shared.DTOs.Attachment;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace ElAmal.Application.Services.Attachments
{
    public class AttachmentServices : IAttachmentServices
    {
        private readonly AttachmentSettings _attachmentSettings;
        private readonly Cloudinary _cloudinary;

        public AttachmentServices(IOptions<AttachmentSettings> attachmentSettings,
            IOptions<CloudinarySettings> cloudinarySettings)
        {
            _attachmentSettings = attachmentSettings.Value;

            var accont = new Account(cloudinarySettings.Value.CloudName,
                                     cloudinarySettings.Value.ApiKey,
                                     cloudinarySettings.Value.ApiSecret);

            _cloudinary = new Cloudinary(accont);
        }

        public async Task<UploadResultDto?> UploadAsync(IFormFile file, string folderName)
        {
            if (file is null || file.Length == 0)
                return null;

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!_attachmentSettings.AllowedImageExtensions.Contains(extension))
                return null;

            if (file.Length > _attachmentSettings.MaxImageSize)
                return null;

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folderName,
                UseFilename = false,
                UniqueFilename = true,
                Overwrite = false
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error is not null)
                throw new Exception("Cloudinary error: " + uploadResult.Error.Message);

            return new UploadResultDto
            {
                ImageUrl = uploadResult.SecureUrl.ToString(),
                PublicId = uploadResult.PublicId
            };
        }


        public async Task DeleteAsync(string? publicId)
        {
            if (string.IsNullOrWhiteSpace(publicId))
                return;

            var deletedParams = new DeletionParams(publicId);

            var deletedImage = await _cloudinary.DestroyAsync(deletedParams);

            if (deletedImage.Result != "ok" && deletedImage.Result != "not found")
            {
                throw new Exception($"Failed to delete image from Cloudinary. Result: {deletedImage.Result}");
            }
        }

    }
}
