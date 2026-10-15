using ElAmal.Core.Contract.Services.Auth;
using ElAmal.Core.Contract.Services.Otp;
using ElAmal.Core.Entities.Identity;
using ElAmal.Core.Helper;
using ElAmal.Shared.DTOs.Identity;
using Matrex.Core.DTOs.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElAmal.Application.Services.Auth
{
    public class UserServices : IUserServices
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuthServices _authServices;
        private readonly IOtpServices _otpServices;

        public UserServices(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAuthServices authServices,
            IOtpServices otpServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authServices = authServices;
            _otpServices = otpServices;
        }
        public async Task<ReturnResult<LoginResponseDto>> LogInAsync(LoginRequestDto loginDto)
        {
            // 1. Check on an email that entered
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
                return ReturnResult<LoginResponseDto>.Fail(message: "Invalid email or password");

            // 2. Check On The Password that entered
            var passwordValidation = await _signInManager.CheckPasswordSignInAsync(user!, loginDto.Password, false);

            if (!passwordValidation.Succeeded)
                return ReturnResult<LoginResponseDto>.Fail(message: "Invalid email or password");

            var (accessToken, refreshToken) = await _authServices.CreateTokenAsync(user, _userManager);

            // 3. Return UserDto
            var userDto = new LoginResponseDto
            {
                Email = user.Email!,
                Token = accessToken,
                RefreshToken = refreshToken
            };


            return ReturnResult<LoginResponseDto>.Success(userDto);
        }

        public async Task<ReturnResult<object>> ForgotPasswordAsync(string companyEmail)
        {
            // 1. Verify Company Email
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == companyEmail);

            if (user is null)
                return ReturnResult<object>.Fail("Invalid  email.");

            // 2. Get configured Recovery Email
            if (string.IsNullOrWhiteSpace(user.RecoveryEmail))
                return ReturnResult<object>
                    .Fail("Recovery email is not configured.");

            // 3. Send OTP to Recovery Email
            var result = await _otpServices.GenerateAndSendOtpAsync(user.RecoveryEmail);

            if (!result.IsSuccess)
                return ReturnResult<object>.Fail(result.Message);

            return ReturnResult<object>.Success("OTP sent successfully");
        }

        public async Task<ReturnResult<object>> ResendForgotPasswordOtpAsync(string companyEmail)
        {
            // 1. Verify the company email
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == companyEmail);

            if (user is null)
                return ReturnResult<object>.Fail("Invalid company email.");

            // 2. Check recovery email
            if (string.IsNullOrWhiteSpace(user.RecoveryEmail))
                return ReturnResult<object>.Fail("Recovery email is not configured.");

            // 3. Resend OTP to the recovery email
            var result = await _otpServices.ResendOtpAsync(user.RecoveryEmail);

            if (!result.IsSuccess)
                return ReturnResult<object>.Fail(result.Message);

            return ReturnResult<object>.Success(null!, "OTP resent successfully.");
        }

        public async Task<ReturnResult<object>> VerifyForgotPasswordOtpAsync(VerifyOtpForForgetPassDto dto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user is null)
                return ReturnResult<object>.Fail("Email not found.");

            var isValid = await _otpServices.VerifyOtpAsync(user.Email!, dto.OtpCode);

            if (!isValid)
                return ReturnResult<object>.Fail("Invalid or expired OTP.");

            return ReturnResult<object>.Success("OTP verified successfully.");
        }

        public async Task<ReturnResult<object>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.Email == resetPasswordDto.Email);

            if (user is null)
                return ReturnResult<object>.Fail("Email not found.");

            if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmPassword)
                return ReturnResult<object>.Fail("Passwords do not match.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(
                user,
                token,
                resetPasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                return ReturnResult<object>.Fail(
                    result.Errors.First().Description);
            }

            return ReturnResult<object>.Success(
                null!,
                "Password reset successfully.");
        }
    }

}
