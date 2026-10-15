using ElAmal.Core.Contract.Services.Auth;
using ElAmal.Core.Helper;
using Matrex.Core.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Application.Services.Auth
{
    public class UserServices : IUserServices
    {
       
        public Task<ReturnResult<LoginResponseDto>> LogInAsync(LoginRequestDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnResult<object>> ForgotPasswordAsync(string companyEmai)
        {
            throw new NotImplementedException();
        }

        public Task<ReturnResult<object>> ResendForgotPasswordOtpAsync(string companyEmai)
        {
            throw new NotImplementedException();
        }
    }
}
