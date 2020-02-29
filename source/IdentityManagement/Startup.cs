using CommandQuery;
using IdentityManagement.Areas.Api.Contracts.Requests;
using IdentityManagement.Configuration;
using IdentityManagement.Domain.Account;
using IdentityManagement.Filters;
using IdentityManagement.Persistence.Identity;
using IdentityManagement.Persistence.Tenancy;
using IdentityManagement.Utilities;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;

namespace IdentityManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUrlHelper>(d => {
                var context = d.GetRequiredService<IActionContextAccessor>();
                return new UrlHelperFactory().GetUrlHelper(context.ActionContext);
            });

            services.AddCommandQueryHandlers();

            services.AddSingleton<TelementryConfiguration>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddSingleton<ISecurityUtils, SecurityUtils>();
            services.AddScoped<IRedirectRequest, RedirectRequest>();
            services.AddScoped<IOpenIdRequest, OpenIdRequest>();
            services.AddScoped<IAuthenticatedUserRequest, AuthenticatedUserRequest>();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddApplicationInsightsTelemetry();

            services.AddResponseCompression();
            services.AddHealthChecks();
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
            });
            services.AddAntiforgery();

            services.AddOpenIdAuthentication(Configuration);
            services.AddTenantDirectory(Configuration);
            
            services.Configure<RouteOptions>(d => {
                d.LowercaseQueryStrings = false;
                d.LowercaseUrls = true;
            });

            services.AddControllersWithViews(d => { 
                d.Filters.Add(new AuthorizeFilter());
                d.Filters.Add(new SecurityHeadersAttribute());
            })
            .SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            InitializeDatabase(app);

            if (environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/errors", "?code={0}");
                app.UseHsts();
            }
            
            app.UseHealthChecks("/diagnostics/health-check");
            app.UseForwardedHeaders();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapFallbackToController("Index", "Home");
                endpoints.MapControllers();
            });
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetRequiredService<TenantUserDbContext>().Database.Migrate();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<TenantDirectoryDbContext>().Database.Migrate();
            }
        }
    }
}
