using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrex.Core.DTOs.Identity
{
    public class ForgotPasswordDto
    {
        [EmailAddress]
        public required string Email { get; set; }
    }
}
