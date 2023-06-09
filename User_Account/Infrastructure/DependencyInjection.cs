﻿using Application.Interfaces;
using Infrastructure.MessageBroker;
using Infrastructure.Security;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtGenerator, JwtGenerator>();

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
            
            busConfigurator.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                var settings = provider.GetRequiredService<MessageBrokerSettings>();
                configurator.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });
            }));
        });

        services.AddTransient<IEventBus, EventBus>();

    }
}