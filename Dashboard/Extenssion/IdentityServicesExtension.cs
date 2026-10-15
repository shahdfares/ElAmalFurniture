using ElAmal.Application.Services.Auth;
using ElAmal.Core.Contract.Services.Auth;
using ElAmal.Core.Entities.Identity;
using ElAmal.Infrastructure.Identity.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Dashboard.Extenssion
{
    public static class IdentityServicesExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ElAmalIdentityDbContext>()
                .AddDefaultTokenProviders(); ;

            services.AddScoped(typeof(IAuthServices), typeof(AuthServices));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromDays(double.Parse(configuration["JWT:ExpireTime"]!)),
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecurityKey"]!))
                };
            });

            return services;
        }

     }
}
