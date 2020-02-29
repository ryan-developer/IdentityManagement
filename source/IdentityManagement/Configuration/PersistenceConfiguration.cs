using System.Reflection;
using IdentityManagement.Persistence.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityManagement.Configuration
{
    public static class PersistenceConfiguration
    {
        public static IServiceCollection AddTenantDirectory(this IServiceCollection services, IConfiguration configuration)
        {
            string migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            string connectionString = configuration.GetConnectionString("TenantDirectory");
            services.AddDbContextPool<TenantDirectoryDbContext>((services, options) => {
                options.UseSqlServer(connectionString,
                    sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
            });

            return services;
        }
    }
}
