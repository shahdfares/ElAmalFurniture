using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Shared.DTOs.Attachment
{
    public class UploadResultDto
    {
        public string ImageUrl { get; set; } = null!;
        public string PublicId { get; set; } = null!;
    }
}
