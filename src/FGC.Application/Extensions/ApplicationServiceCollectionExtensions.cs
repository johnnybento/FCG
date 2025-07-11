
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FGC.Application.Extensions;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
        );

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }
}