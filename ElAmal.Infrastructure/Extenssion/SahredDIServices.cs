using ElAmal.Application.Services.Attachments;
using ElAmal.Application.Services.Auth;
using ElAmal.Application.Services.Category;
using ElAmal.Application.Services.Mail;
using ElAmal.Application.Services.Otp;
using ElAmal.Application.Services.products;
using ElAmal.Core.Contract.Repository;
using ElAmal.Core.Contract.Services.Attachment;
using ElAmal.Core.Contract.Services.Auth;
using ElAmal.Core.Contract.Services.Category;
using ElAmal.Core.Contract.Services.Mail;
using ElAmal.Core.Contract.Services.Otp;
using ElAmal.Core.Contract.Services.Products;
using ElAmal.Core.Contract.Services.SpecProperty;
using ElAmal.Core.Contract.Services.TechnicalSpecification;
using ElAmal.Core.Entities.Mail;
using ElAmal.Core.Entities.Products.Attachments;
using ElAmal.Core.Helper;
using ElAmal.Infrastructure.Data;
using ElAmal.Infrastructure.Identity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Runtime.CompilerServices;

namespace ElAmal.Shared.Extenssion
{
    public  static class SahredDIServices
    {
        public static IServiceCollection AddSahredDIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextServices(configuration);
            services.AddUserDefinedService();
            services.AddAutomapperServices();
            services.AddAttachmentServices(configuration);
            services.AddMailSettingsServices(configuration);
            services.AddRedisServices(configuration);

            return services;
        }

        private static IServiceCollection AddDbContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ElAmalDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ElAmalIdentityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("IdentityConnection")));

            return services;
        }
        private static IServiceCollection AddUserDefinedService(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IElAmalContextInitializer, ElAmalContextInitializer>();
            services.AddScoped<IProductServices, ProductServices>();
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<IAttachmentServices, AttachmentServices>();
            services.AddScoped<ISpecificatonPropertyServices, SpecificatonPropertyServices>();
            services.AddScoped(typeof(IUserServices), typeof(UserServices));
            services.AddScoped(typeof(IAuthServices), typeof(AuthServices));

            services.AddTransient(typeof(IMailSettingsService), typeof(MailSettingsService));
            services.AddTransient(typeof(IOtpServices), typeof(OtpServices));

            return services;
        }
        private static IServiceCollection AddAutomapperServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
        private static IServiceCollection AddAttachmentServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AttachmentSettings>(configuration.GetSection("AttachmentSettings"));

            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

            return services;
        }
        private static IServiceCollection AddMailSettingsServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            return services;
        }
        private static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>((serverProvider) =>
            {
                // Add Connection String
                var connection = configuration.GetConnectionString("Redis");

                return ConnectionMultiplexer.Connect(connection!);
            });

            return services;
        }

    }
}
