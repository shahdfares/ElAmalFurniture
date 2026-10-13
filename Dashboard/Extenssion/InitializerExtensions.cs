using Dashboard;
using ElAmal.Core.Contract.Repository;

namespace ElAmal.Extensions
{
    public static class InitializerExtensions
    {
        public static async Task<WebApplication> InitializerElAmalContextAsync(this WebApplication app)
        {

           using var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider;
            var configuration = service.GetRequiredService<IConfiguration>();

            var elAmalContextInitializer = service.GetRequiredService<IElAmalContextInitializer>();

            var loggerFactory = service.GetRequiredService<ILoggerFactory>();

            try
            {
                await elAmalContextInitializer.UpdateDatabaseAsync();
            }
            catch (Exception ex)
            {

                // 1, Grt Error 
                var logger = loggerFactory.CreateLogger<Program>();

                // 2. Set Friendly Request

                logger.LogError(ex, "An error has been occurred during the migration");
            }

            return app;
        }
    }
}
