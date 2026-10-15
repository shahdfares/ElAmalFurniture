using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Shared.DTOs.TechnicalSpecification
{
    public class ProductTechnicalSpecificationRequest
    {
        public Guid? ProductId { get; set; }
        public Guid TechnicalSpecificationId { get; set; }
        public string Value { get; set; } = null!;
    }
}
