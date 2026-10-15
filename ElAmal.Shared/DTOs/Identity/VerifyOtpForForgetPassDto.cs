using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Shared.DTOs.Identity
{
    public class VerifyOtpForForgetPassDto
    {
        public required string Email { get; set; }
        public required string OtpCode { get; set; }
    }
}
