using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace WebAPI.Checks;

public static class HealthCheckExtensions
{
    public static IServiceCollection AddHealthCheckService(this IServiceCollection service, 
        IConfiguration configuration)
    {
        service.AddHealthChecks()
            .AddDbCheck(ConnectionStringEcommerce(configuration), "Ecommerce DB");
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

    private static IHealthChecksBuilder AddDbCheck(this IHealthChecksBuilder builder, string connectionString,
        string description)
    {
        return builder.AddTypeActivatedCheck<DbConnectionHealthCheck>(description, connectionString);
    }

    private static string ConnectionStringEcommerce(IConfiguration configuration)
    {
        return configuration.GetConnectionString("PostGreSQL");
    }
}