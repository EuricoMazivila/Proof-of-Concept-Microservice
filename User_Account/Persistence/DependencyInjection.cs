using System.Text;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostGreSQL")));
        services.AddIdentityServices(configuration);
        return services;
    }

    private static IServiceCollection AddIdentityServices(this IServiceCollection services, 
        IConfiguration config)
    {
        var builder = services.AddIdentityCore<AppUser>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
        });

        builder = new IdentityBuilder(builder.UserType, builder.Services);

        builder.AddEntityFrameworkStores<DataContext>()
            .AddDefaultTokenProviders();

        builder.AddSignInManager<SignInManager<AppUser>>();

        var tokenValidationParams = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Token:TokenSecret").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        services.AddSingleton(tokenValidationParams);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Token:TokenSecret").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        
        return services;
    }

    public static WebApplication AddUpdateMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            
        try
        {
            var context = services.GetRequiredService<DataContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var env = services.GetService<IWebHostEnvironment>();
            context.Database.MigrateAsync().Wait();
            DataContextSeed.SeedAsync(context, loggerFactory, userManager, env!).Wait();

        }
        catch (Exception e)
        {
            Console.WriteLine("An error occured during creating data from seeds");
        }

        return app;
    }
}