using Infrastructure.MessageBroker;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        ConfigureMessageBroker(services, configuration);
        return services;
    }

    private static void ConfigureMessageBroker(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MessageBrokerSettings>(
            configuration.GetSection("MessageBroker"));
        
        services.AddSingleton(sp =>
            sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
        
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            busConfigurator.AddConsumer<UserCreatedEventConsumer>();
            
            busConfigurator.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var settings = provider.GetRequiredService<MessageBrokerSettings>();
                
                cfg.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });
                cfg.ReceiveEndpoint("userCreatedQueue", ep =>
                {
                    ep.PrefetchCount = 10;
                    ep.UseMessageRetry(r => r.Interval(2,100));
                    ep.ConfigureConsumer<UserCreatedEventConsumer>(provider);
                });
            }));
        });
    }
}