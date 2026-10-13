
using Dashboard.Extenssion;
using ElAmal.Extensions;

namespace Dashboard
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            #region Services Configurations


            webApplicationBuilder.Services.AddDashboardService(webApplicationBuilder.Configuration);

            #endregion

            var app = webApplicationBuilder.Build();

            #region Database Initialization

            await app.InitializerElAmalContextAsync();

            #endregion

            #region Kestrel Configurations

            app.MiddlewareConfigurations();

            #endregion

            app.Run();
        }
    }
}
