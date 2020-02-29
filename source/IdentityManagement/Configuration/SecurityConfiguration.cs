using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using IdentityManagement.Persistence.Identity;
using IdentityManagement.Persistence.Identity.Entities;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.AspNetIdentity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using static IdentityModel.OidcConstants;

namespace IdentityManagement.Configuration
{
    public static class SecurityConfiguration
    {
        private const string AUTHSCHEME_AAD = "aad";
        private const string AUTHSCHEME_GOOGLE = "google";

        public static IServiceCollection AddOpenIdAuthentication(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.AccessDeniedPath = "/errors/denied";
                options.LoginPath = "/account/sign-in";
                options.LogoutPath = "/account/sign-out";
            });

            services
                .AddIdentityServerAuthentication(configuration)
                .AddLocalApiAuthentication()
                .AddAuthorization();

            services.AddOidcStateDataFormatterCache(AUTHSCHEME_AAD);
            services.AddAuthentication(options => {
                options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = IdentityServerConstants.DefaultCookieAuthenticationScheme;
            })
            .AddAzureADAuthentication(configuration)
            .AddGoogleAuthentication(configuration);

            return services;
        }

        private static AuthenticationBuilder AddAzureADAuthentication(
            this AuthenticationBuilder authBuilder, 
            IConfiguration configuration)
        {
            authBuilder.AddOpenIdConnect(AUTHSCHEME_AAD, "Azure Active Directory", options =>
            {
                options.Scope.Clear();
                configuration.Bind("AuthProviders:AzureAD", options);
                options.ResponseType = "id_token";
                options.ResponseMode = ResponseModes.FormPost;
                options.Events.OnAuthenticationFailed = async (context) => {
                    Console.WriteLine(context.Scheme);
                    await Task.CompletedTask;
                };
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.SignedOutRedirectUri = "/account/sign-in";
                options.SaveTokens = true;
                options.UseTokenLifetime = false;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateActor = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = false,
                    ValidateLifetime = false,
                    ValidateAudience = false,
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role,
                };
            });

            return authBuilder;
        }

        private static AuthenticationBuilder AddGoogleAuthentication(
            this AuthenticationBuilder authBuilder,
            IConfiguration configuration)
        {
            authBuilder.AddGoogle(AUTHSCHEME_GOOGLE, options => {
                options.Scope.Clear();
                configuration.Bind("AuthProviders:Google", options);
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                //options.SaveTokens = true;
            });
            return authBuilder;
        }

        private static IServiceCollection AddIdentityServerAuthentication(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            string identityConnectionString = configuration.GetConnectionString("Identity");
            string migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddDataProtection()
                .SetApplicationName("identity-management");

            services.AddDbContextPool<TenantUserDbContext>((services, options) => {
                options.UseSqlServer(identityConnectionString, 
                    sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
            });


            services.AddIdentity<TenantUserEntity, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<TenantUserDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.User.RequireUniqueEmail = true;
            });

            IIdentityServerBuilder identityBuilder = services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "~/account/sign-in";
                options.UserInteraction.LogoutUrl = "~/account/sign-out";
                options.UserInteraction.ErrorUrl = "~/error";
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Authentication.CookieSlidingExpiration = true;
            });

            identityBuilder.AddConfigurationStore(options => {
                options.ConfigureDbContext = context => 
                    context.UseSqlServer(identityConnectionString, 
                        sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
            });

            identityBuilder.AddOperationalStore(options => {

                options.ConfigureDbContext = context => 
                    context.UseSqlServer(identityConnectionString, 
                        sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 7200;
            });
            identityBuilder.AddAspNetIdentity<TenantUserEntity>()
                .AddProfileService<ProfileService<TenantUserEntity>>()
                .AddInMemoryCaching();

            var certificate = GetSigningCertificate(configuration);
            X509SigningCredentials signingCredentials = new X509SigningCredentials(certificate);
            identityBuilder.AddSigningCredential(signingCredentials);

            return services;
        }

        private static X509Certificate2 GetSigningCertificate(IConfiguration configuration)
        {
            var contentRoot = configuration.GetValue<string>(WebHostDefaults.ContentRootKey);

            IConfiguration signingCertConfig = configuration.GetSection("OpenId:SigningCert");
            string thumbPrint = signingCertConfig.GetValue<string>("Thumbprint");
            string password = signingCertConfig.GetValue<string>("Password");

            X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);
            var certCollection = certStore.Certificates.Find(
                X509FindType.FindByThumbprint,
                thumbPrint, 
                validOnly: false);

            X509Certificate2 certificate = null;
            if (certCollection.Count > 0)
            {
                certificate = certCollection[0];
            }
            else
            {
                string certLocation = Path.Combine(contentRoot, "Configuration", "Certificates", "IdentityServer.pfx");
                certificate = new X509Certificate2(certLocation, password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            }
            return certificate;
        }
    }
}
