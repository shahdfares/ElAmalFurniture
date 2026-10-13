using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Shared.DTOs.TechnicalSpecification
{
    public class TechniacalSpecRequestDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;
    }
}
