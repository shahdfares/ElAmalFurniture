using ElAmal.Shared.DTOs.TechnicalSpecification;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Shared.DTOs.Product
{
    public class ProductRequestDto
    {
        //Basic Information
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public string? Descripton { get; set; }
        public Guid? CategoryId { get; set; }

        // Material Construction
        public List<ProductTechnicalSpecificationRequest> technicalSpecificationRequests { get; set; } = [];

        // Images
        public List<IFormFile> Images { get; set; } = [];

    }
}
