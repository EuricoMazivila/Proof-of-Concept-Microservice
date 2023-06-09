﻿using Microsoft.OpenApi.Models;
using WebAPI.Serialization;
using WebAPI.Serialization.Results;

namespace WebAPI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebAPI(this IServiceCollection services)
    {
        SwaggerConfiguration(services);
        services.AddSerializationResult();
        return services;
    }

    private static void SwaggerConfiguration(IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "User Account Service",
                Version = "v1"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
                              + "\r\n\r\n Put **_ONLY_** your JWT Bearer token in"
                              + "the text input below."
                              + "\r\n\r\nExample: \"12345abcdef\""
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
    
    private static IServiceCollection AddSerializationResult(this IServiceCollection services)
    {
        services
            .AddTransient<IResultSerializationStrategy, SerializationResultSuccess>()
            .AddTransient<IResultSerializationStrategy, SerializationResultInternalError>()
            .AddTransient<IResultSerializationStrategy, SerializationResultValidationError>()
            .AddTransient<IResultSerializationStrategy, SerializationResultApplicationError>()
            .AddTransient<IResultSerializationStrategy, SerializationResultNotFoundError>()
            .AddTransient<IResultSerializationStrategy, SerializationResultUnauthorizedError>()
            .AddTransient<IResultSerializationStrategy, SerializationResultConflictError>();

        return services;
    }
}