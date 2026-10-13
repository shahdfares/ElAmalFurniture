using ElAmal.Shared.Middleware;

namespace Dashboard.Extenssion
{
    public static class MiddlewareConfiguration
    {
        public static WebApplication MiddlewareConfigurations(this WebApplication app)
        {
            app.UseMiddleware<ExeptionMiddleware>();

            // Configure the HTTP request pipeline.

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                // Enable the "Authorize" button in Swagger UI for JWT authentication
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Dashboard API v1");
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
            });

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("default");

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}
