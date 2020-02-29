using IdentityManagement.Persistence.Identity.Entities;
using IdentityModel;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityManagement.Domain.Account
{
    public class OpenIdRequest : IOpenIdRequest
    {
        private const string DEFAULT_ROLE = "RegisteredUser";

        private readonly UserManager<TenantUserEntity> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IHttpContextAccessor contextAccessor;

        public string Id { get; private set; }
        public string Email { get; private set; }
        public string UserName { get; private set; }
        public string IdentitySource { get; private set; }
        public string IdentityProvider { get; private set; }
        public string ExternalUserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Name { get; private set; }
        public string Locale { get; private set; }
        public string TenantId { get; private set; }
        public string ProfilePictureUrl { get; private set; }
        public bool IsAuthenticated { get; private set; }

        public IReadOnlyList<string> Roles { get; private set; }
        public IReadOnlyList<Claim> Claims { get; private set; }

        private TenantUserEntity tenantUser;

        public OpenIdRequest(
            UserManager<TenantUserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor contextAccessor)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.contextAccessor = contextAccessor;

            InitializeClaims(contextAccessor.HttpContext.User);
        }

        private List<string> roles;
        private List<Claim> claims;

        private void InitializeClaims(ClaimsPrincipal principal)
        {
            claims = new List<Claim>();
            roles = new List<string>();

            Claims = new ReadOnlyCollection<Claim>(claims);
            Roles = new ReadOnlyCollection<string>(roles);

            bool IsAuthenticated = principal.Identity?.IsAuthenticated ?? false; 
            if(!IsAuthenticated)
            {
                roles.Add("Anonymous");
                return;
            }
            
            string emailAddress = principal.FindFirstValue(JwtClaimTypes.Email) ?? 
                                  principal.FindFirstValue("unique_name");

            Email = emailAddress;
            UserName = emailAddress;
            IdentitySource = principal.FindFirstValue(JwtClaimTypes.Issuer);
            IdentityProvider = principal.FindFirstValue(JwtClaimTypes.IdentityProvider);
            ExternalUserId = principal.FindFirstValue(JwtClaimTypes.Subject);
            FirstName = principal.FindFirstValue(JwtClaimTypes.GivenName);
            LastName = principal.FindFirstValue(JwtClaimTypes.FamilyName);
            Name = principal.FindFirstValue(JwtClaimTypes.Name);
            Locale = principal.FindFirstValue(JwtClaimTypes.Locale);
            ProfilePictureUrl = principal.FindFirstValue(JwtClaimTypes.Picture);
            
            roles.Add(DEFAULT_ROLE);

            TryAddClaim(JwtClaimTypes.GivenName, FirstName);
            TryAddClaim(JwtClaimTypes.FamilyName, LastName);
            TryAddClaim(JwtClaimTypes.Name, Name);
            TryAddClaim(JwtClaimTypes.Email, Email);
            TryAddClaim(JwtClaimTypes.PreferredUserName, Email);
            TryAddClaim(JwtClaimTypes.GivenName, FirstName);
            TryAddClaim(JwtClaimTypes.Picture, ProfilePictureUrl);
            TryAddClaim(JwtClaimTypes.IdentityProvider, IdentityProvider);
            TryAddClaim(JwtClaimTypes.Issuer, IdentitySource);
            TryAddClaim(JwtClaimTypes.Role, DEFAULT_ROLE);
            TryAddClaim("external_id", ExternalUserId);

            Claims = new ReadOnlyCollection<Claim>(claims);
            Roles = new ReadOnlyCollection<string>(roles);
        }

        private void TryAddClaim(string claimType, string value)
        {
            if(!string.IsNullOrWhiteSpace(value))
            {
                claims.Add(new Claim(claimType, value));
            }
        }

        public async Task<TenantUserEntity> RegisterUser()
        {
            TenantUserEntity user = userManager.Users
                .Where(d => d.Email == Email)
                .Where(d => d.IdentitySource == IdentitySource)
                .Where(d => d.IdentityProvider == IdentityProvider)
                .FirstOrDefault();

            if(user != null)
            {
                tenantUser = user;
                return tenantUser;
            }

            tenantUser = new TenantUserEntity();
            tenantUser.Id = Guid.NewGuid().ToString();

            tenantUser.Email = Email;
            tenantUser.UserName = Email;
            tenantUser.ExternalUserId = ExternalUserId;
            tenantUser.IdentitySource = IdentitySource;
            tenantUser.IdentityProvider = IdentityProvider;

            IdentityResult userResult = await userManager.CreateAsync(tenantUser);
            if (!userResult.Succeeded) {
                throw new Exception(string.Join(" ", userResult.Errors));
            }

            IdentityResult claimsResult = await userManager.AddClaimsAsync(tenantUser, claims);
            if (!userResult.Succeeded)
            {
                throw new Exception(string.Join(" ", userResult.Errors));
            }

            return tenantUser;
        }
    }
}
