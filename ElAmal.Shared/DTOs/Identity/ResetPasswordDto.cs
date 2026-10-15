using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Shared.DTOs.Identity
{
    public class ResetPasswordDto
    {
        [EmailAddress]
        public required string Email { get; init; }

        [Required(ErrorMessage = "Password is Required")]
        [MinLength(5, ErrorMessage = "Minimum Password Length is 5")]
        [DataType(DataType.Password)]
        public required string NewPassword { get; init; }


        [Required(ErrorMessage = "Confirm Password is Required")]
        [Compare(nameof(NewPassword), ErrorMessage = "Confirm Password does not match Password")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; init; }
    }
}
