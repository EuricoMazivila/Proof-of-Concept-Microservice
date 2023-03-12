using Application.Behaviors;
using Application.Helpers;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequisitionValidationPipelineBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionPipelineBehavior<,>));
        
        var assembly = typeof(DependencyInjection).Assembly;
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));

        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssembly(assembly);
        
        services.AddAutoMapper(typeof(MappingProfiles));
        
        return services;
    } 
}