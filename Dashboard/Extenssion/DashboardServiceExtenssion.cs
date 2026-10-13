using ElAmal.Shared.Errors;
using ElAmal.Shared.Extenssion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace Dashboard.Extenssion
{
    public static class DashboardServiceExtenssion
    {
        public static IServiceCollection AddDashboardService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBuiltInServices();  
            services.AddSwaggerServices();  
            services.AddValidationErrorHandling();

            services.AddSahredDIServices(configuration);
        

            return services;
        }

        private static IServiceCollection AddBuiltInServices(this IServiceCollection services)
        {
            // Add services to the container.

            services.AddControllers();
            
            return services;
        }
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });


            return services;
        }
        private static IServiceCollection AddValidationErrorHandling(this IServiceCollection services)
        {

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionscontext) =>
                {
                    var errors = actionscontext.ModelState
                    .Where(e => e.Value!.Errors.Count() > 0)
                    .ToDictionary(
                        x => x.Key,
                        x => x.Value!.Errors
                               .Select(e => e.ErrorMessage)
                               .ToArray());

                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }

    }
}
