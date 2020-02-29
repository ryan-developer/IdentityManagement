using IdentityManagement.Areas.Api.ViewModels;
using IdentityManagement.Utilities;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityManagement.Areas.Api.Contracts.Requests
{
    public interface IApplicationRegistrationRequest
    {
        Task<NewClientIdentity> InvokeAsync(ApplicationRegistration registration);
    }

    public class ApplicationRegistrationRequest : IApplicationRegistrationRequest
    {
        protected readonly ConfigurationDbContext configContext;
        protected readonly ISecurityUtils securityUtils;
        protected readonly IHttpContextAccessor contextAccessor;
        protected readonly ClaimsPrincipal user;

        public ApplicationRegistrationRequest(
            ConfigurationDbContext configContext,
            IHttpContextAccessor contextAccessor,
            ISecurityUtils securityUtils)
        {
            this.configContext = configContext;
            this.securityUtils = securityUtils;
            this.contextAccessor = contextAccessor;
            this.user = contextAccessor.HttpContext.User;
        }

        public async Task<NewClientIdentity> InvokeAsync(ApplicationRegistration registration)
        {
            Guid clientId = Guid.NewGuid();
            string name = registration.Name;
            string tenantId = user?.FindFirstValue("tenantid");

            string primarySecret = securityUtils.CreateRandomKey();
            string secondarySecret = securityUtils.CreateRandomKey();

            Client client = new Client();
            client.AccessTokenType = AccessTokenType.Jwt;
            switch (registration.ApplicationType)
            {
                case AuthenticationType.ClientCredentials:
                    client.AllowedGrantTypes = GrantTypes.ClientCredentials;
                    client.AlwaysSendClientClaims = true;
                    break;
                case AuthenticationType.InteractiveWeb:
                    client.AllowedGrantTypes = GrantTypes.Hybrid;
                    client.AllowAccessTokensViaBrowser = true;
                    client.RequireConsent = registration.RequireConsent;
                    break;
                case AuthenticationType.UserPassword:
                    client.AllowedGrantTypes = GrantTypes.ResourceOwnerPassword;
                    break;
            }
            client.AllowOfflineAccess = true;
            client.AllowedScopes = registration.Scopes;

            client.RequireClientSecret = true;
            client.ClientId = clientId.ToString();
            client.ClientSecrets = new List<Secret> {
                new Secret(primarySecret.Sha256(), "Primary"),
                new Secret(secondarySecret.Sha256(), "Secondary"),
            };

            client.ProtocolType = "oidc";
            client.ClientName = name;
            client.AccessTokenLifetime = (int)TimeSpan.FromHours(1).TotalSeconds;
            client.RefreshTokenUsage = TokenUsage.OneTimeOnly;
            client.IncludeJwtId = true;
            client.Enabled = true;
            client.Claims = new List<Claim>() {
                new Claim("tenantid", tenantId)
            };

            var entity = client.ToEntity();
            await configContext.Clients.AddAsync(entity);
            await configContext.SaveChangesAsync();

            return new NewClientIdentity
            {
                ClientId = clientId.ToString(),
                PrimarySecret = primarySecret,
                SecondarySecret = secondarySecret
            };
        }
    }
}
