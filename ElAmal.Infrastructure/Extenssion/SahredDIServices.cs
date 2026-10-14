using ElAmal.Application.Services.Attachments;
using ElAmal.Application.Services.Category;
using ElAmal.Application.Services.products;
using ElAmal.Core.Contract.Repository;
using ElAmal.Core.Contract.Services.Attachment;
using ElAmal.Core.Contract.Services.Category;
using ElAmal.Core.Contract.Services.Products;
using ElAmal.Core.Contract.Services.SpecProperty;
using ElAmal.Core.Contract.Services.TechnicalSpecification;
using ElAmal.Core.Entities.Products.Attachments;
using ElAmal.Core.Helper;
using ElAmal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            return services;
        }

        private static IServiceCollection AddDbContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ElAmalDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

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

    }
}
