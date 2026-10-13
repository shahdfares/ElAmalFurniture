using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Entities.Products.Attachments
{
    public class AttachmentSettings
    {
        public int MaxImageSize { get; set; }
        public List<string> AllowedImageExtensions { get; set; } = [];
    }
}
