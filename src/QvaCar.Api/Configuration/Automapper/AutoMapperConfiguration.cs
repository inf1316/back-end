using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace QvaCar.Api.Configuration
{
    internal static class AutoMapperConfiguration
    {
       public static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(new Assembly[]
            {
                Assembly.GetExecutingAssembly(),                    
            });
            return services;
        }        
    }
   
}