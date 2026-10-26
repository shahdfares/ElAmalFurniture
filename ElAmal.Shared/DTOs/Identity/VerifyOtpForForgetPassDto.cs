using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Shared.DTOs.Identity
{
    public class VerifyOtpForForgetPassDto
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string OtpCode { get; set; }
    }
}
