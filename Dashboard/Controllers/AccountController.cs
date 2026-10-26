using ElAmal.Core.Contract.Services.Auth;
using ElAmal.Core.Helper;
using ElAmal.Shared.DTOs.Identity;
using ElAmal.Shared.Errors;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;

namespace Dashboard.Controllers
{
    public class AccountController(IUserServices _userServices) : BaseController
    {
        [HttpPost("login")]  //Post: /api/dashboard/Account/login
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Email) && string.IsNullOrEmpty(loginDto.Email))
                return BadRequest(new ApiResponse(400));

            var user = await _userServices.LogInAsync(loginDto);

            if (user is null)
                return Unauthorized(new ApiResponse(401));

            return Ok(user);
        }

        [HttpPost("forgot-password")]  //Post: /api/dashboard/Account/forgot-password
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (dto is null)
                return BadRequest(new ApiResponse(400));

            var result = await _userServices.ForgotPasswordAsync(dto.Email);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse(400, result.Message));

            return Ok(result);
        }

        [HttpPost("resend-forgot-password-otp")]
        public async Task<ActionResult> ResendForgotPasswordOtp([FromBody] ForgotPasswordDto dto)
        {
            if (dto is null)
                return BadRequest(new ApiResponse(400));

            var result = await _userServices.ResendForgotPasswordOtpAsync(dto.Email);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse(400));

            return Ok(result);
        }

        [HttpPost("verify-forgot-password-otp")]
        public async Task<ActionResult> VerifyForgotPasswordOtp([FromBody] VerifyOtpForForgetPassDto dto)
        {
            if (dto is null)
                return BadRequest(new ApiResponse(404));

            var result = await _userServices.VerifyForgotPasswordOtpAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse(400));

            return Ok(result);
        }


        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (dto is null)
                return BadRequest(new ApiResponse(400));

            var result = await _userServices.ResetPasswordAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new ApiResponse(400));

            return Ok(result);
        }
    }
}
