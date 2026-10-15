using ElAmal.Core.Helper;
using ElAmal.Shared.DTOs.Identity;
using Matrex.Core.DTOs.Identity;

namespace ElAmal.Core.Contract.Services.Auth
{
    public interface IUserServices
    {
        Task<ReturnResult<LoginResponseDto>> LogInAsync(LoginRequestDto loginDto);
        Task<ReturnResult<object>> ForgotPasswordAsync(string companyEmai);
        Task<ReturnResult<object>> ResendForgotPasswordOtpAsync(string companyEmai);
        Task<ReturnResult<object>> VerifyForgotPasswordOtpAsync(VerifyOtpForForgetPassDto dto);
        Task<ReturnResult<object>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
