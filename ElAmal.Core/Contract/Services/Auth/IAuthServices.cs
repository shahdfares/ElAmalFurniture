using ElAmal.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace ElAmal.Core.Contract.Services.Auth
{
    public interface IAuthServices
    {
        Task<(string AccessToken, string RefreshToken)> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager);
    }
}
