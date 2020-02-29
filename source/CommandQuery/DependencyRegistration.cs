using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace CommandQuery
{
    public static class DependencyRegistration
    {
        public static IServiceCollection AddCommandQueryHandlers(this IServiceCollection services)
        {
            var domain = AppDomain.CurrentDomain.GetAssemblies();
            var types = domain.SelectMany(d => d.GetTypes());

            var instanceTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(d => d.GetTypes())
                .Where(d => 
                    !d.IsInterface && 
                    !d.IsAbstract && 
                    d.BaseType != null && 
                    d.GetInterfaces()
                        .Any(i => i == typeof(ICQRS)));

            foreach(Type type in instanceTypes)
            {
                services.AddScoped(type);
            }

            return services;
        }
    }
}