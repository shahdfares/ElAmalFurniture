
using ElAmal.Extensions;

namespace ElAmal
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplication = WebApplication.CreateBuilder(args);

            #region Configuration Services

            webApplication.Services.AddApplicationService(webApplication.Configuration);
           

            #endregion 

            var app = webApplication.Build();

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
