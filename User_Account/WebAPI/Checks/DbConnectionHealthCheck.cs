using System.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace WebAPI.Checks;

public class DbConnectionHealthCheck : IHealthCheck
{
    private readonly string _fullConnectionString;

    public DbConnectionHealthCheck(string fullConnectionString)
    {
        _fullConnectionString = fullConnectionString;
    }
    
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        var data = new Dictionary<string, object>
        {
            { "connection_string", RemoveUsernameAndPassword(_fullConnectionString) }
        };
        
        try
        {
            var sqlConnection = new NpgsqlConnection(_fullConnectionString);
            await sqlConnection.OpenAsync(cancellationToken);
            await sqlConnection.CloseAsync();

            return HealthCheckResult.Healthy("Conexão efetuada com sucesso!", data);
        }
        catch (SqlException exception)
        {
            data.Add("stack_trace", exception.StackTrace ?? "empty stack trace");
            data.Add("message", exception.Message);
            return HealthCheckResult.Unhealthy($"Não foi possível se conectar.", data: data);
        }
    }

    private static string RemoveUsernameAndPassword(string connectionString)
    {
        bool StartsWith(string str, params string[] patterns)
            => patterns.Any(p => str.TrimStart().StartsWith(p, StringComparison.OrdinalIgnoreCase));

        return string.Join(';', connectionString
            .Split(';')
            .Where(str => !StartsWith(str, "p", "u")));
    }
}