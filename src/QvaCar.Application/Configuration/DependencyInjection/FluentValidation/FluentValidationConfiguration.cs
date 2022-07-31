using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace QvaCar.Application.Configuration
{
    internal static class FluentValidationConfiguration
    {
        public static IServiceCollection AddCustomFluentValidation(this IServiceCollection services)
        {
           services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}