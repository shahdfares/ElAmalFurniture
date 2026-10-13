using ElAmal.Shared.DTOs.Attachment;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Contract.Services.Attachment
{
    public interface IAttachmentServices
    {
        Task<UploadResultDto?> UploadAsync(IFormFile file, string folderName);
        Task DeleteAsync(string? publicId);
    }
}
