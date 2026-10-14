using ElAmal.Core.Contract.Services.Auth;
using ElAmal.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ElAmal.Application.Services.Auth
{
    public class AuthServices : IAuthServices
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthServices(IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<(string AccessToken, string RefreshToken)> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            // Private Claims (User-Defined)
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.GivenName,user.UserName!),
                new Claim(ClaimTypes.Email,user.Email!)
            };

            // Get User Role
            var UserRole = await userManager.GetRolesAsync(user);

            foreach (var role in UserRole)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecurityKey"]!));

            if (authKey.Key.Length == 0)
                throw new InvalidOperationException("JWT SecurityKey is not configured!");

            // Create Token Object 
            var token = new JwtSecurityToken(
                // Register Claims
                issuer: _configuration["JWT:issuer"],
                audience: _configuration["JWT:audience"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:ExpireTime"]!)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                );

            //Create Token
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // Generate Refresh Token
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(
                double.Parse(_configuration["JWT:RefreshExpireDays"]!));

            await userManager.UpdateAsync(user);

            return (AccessToken: accessToken, RefreshToken: refreshToken);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
    
}
