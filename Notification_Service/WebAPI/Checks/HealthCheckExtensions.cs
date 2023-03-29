using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace WebAPI.Checks;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthCheckService(this IServiceCollection service)
    {
        service.AddHealthChecks();
        return service;
    }

    public static void AddHealthCheckApp(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/api/health",
            new HealthCheckOptions
            {
                Predicate = p => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
    }
}