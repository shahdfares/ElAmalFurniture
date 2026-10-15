using ElAmal.Core.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElAmal.Core.Contract.Services.Otp
{
    public interface IOtpServices
    {
        Task<ReturnResult<object>> GenerateAndSendOtpAsync(string email);
        Task<bool> VerifyOtpAsync(string email, string otp);
        Task<ReturnResult<object>> ResendOtpAsync(string email);
    }
}
