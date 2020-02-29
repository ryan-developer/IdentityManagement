using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace IdentityManagement.Areas.Api.Contracts.Responses
{
    public class User
    {
        public string Email { get; set; }

        public string Id { get; set; }

        public string TenantID { get; set; }

        public string IdentityProvider => 
            IdentityClaims?.FirstOrDefault(d => d.ClaimType == JwtClaimTypes.IdentityProvider)?.ClaimValue;

        public string GivenName =>
            IdentityClaims?.FirstOrDefault(d => d.ClaimType == JwtClaimTypes.GivenName)?.ClaimValue;

        public string FamilyName =>
            IdentityClaims?.FirstOrDefault(d => d.ClaimType == JwtClaimTypes.FamilyName)?.ClaimValue;

        public IEnumerable<string> Roles =>
            IdentityClaims?.Where(d => d.ClaimType == JwtClaimTypes.Role).Select(d => d.ClaimValue);

        [JsonIgnore]
        public List<IdentityUserClaim<string>> IdentityClaims { get; set; }

        [JsonIgnore]
        public List<IdentityUserRole<string>> IdentityRoles { get; set; }
    }
}
