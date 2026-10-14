
using ElAmal.Extensions;

namespace ElAmal
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Configuration Services

            webApplicationBuilder.Services.AddApplicationService(webApplicationBuilder.Configuration);

            // webApplicationBuilder.Services.AddIdentityServices(webApplicationBuilder.Configuration);

            #endregion

            var app = webApplicationBuilder.Build();

            #region Database Initialization

            await app.InitializerElAmalContextAsync();

            #endregion

            #region Kestral Configuration

            app.MiddlewareConfigurations();

            #endregion

            app.Run();
        }
    }
}
