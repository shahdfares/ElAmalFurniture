using ElAmal.Shared.DTOs.Image;
using ElAmal.Shared.DTOs.TechnicalSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Shared.DTOs.Product
{
    public class ProductToReturnDto
    {
        public required string Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public string? Slug { get; set; }

        public Guid CategoryId { get; set; }
        public string Category { get; set; } = null!;
        public string CategorySlug { get; set; } = null!;

        public List<ImageResponseDto> ProductImages { get; set; } = new ();
        public List<TechnicalSpecificationResponse> TechnicalSpecification { get; set; } = new();
    }
}
