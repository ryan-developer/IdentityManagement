using IdentityManagement.Areas.Account.Contracts;
using IdentityManagement.Persistence;
using IdentityManagement.Persistence.Identity;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityManagement.Areas.Api.Contracts.Requests
{
    public interface IAuthenticatedUserRequest
    {
        UserAuthResponse Invoke();
    }

    public class AuthenticatedUserRequest : IAuthenticatedUserRequest
    {
        private IHttpContextAccessor contextAccessor;
        private TenantUserDbContext dbContext;

        public AuthenticatedUserRequest(
            IHttpContextAccessor contextAccessor,
            TenantUserDbContext dbContext)
        {
            this.contextAccessor = contextAccessor;
            this.dbContext = dbContext;
        }

        public UserAuthResponse Invoke()
        {
            ClaimsPrincipal user = contextAccessor.HttpContext.User;
            var responseContract = new UserAuthResponse();

            responseContract.EmailAddress = user.FindFirstValue(JwtClaimTypes.Email);
            responseContract.FirstName = user.FindFirstValue(JwtClaimTypes.GivenName);
            responseContract.LastName = user.FindFirstValue(JwtClaimTypes.FamilyName);
            responseContract.IsAuthenticated = user.Identity?.IsAuthenticated ?? false;

            if (responseContract.IsAuthenticated)
            {
                string gravatarId = GenerateGravatarHash(responseContract.EmailAddress);
                responseContract.GravatarUrl = $"https://www.gravatar.com/avatar/{gravatarId}?d=mp";
            }
            return responseContract;
        }

        private string GenerateGravatarHash(string emailAddress)
        {
            var md5 = MD5.Create();
            byte[] rawHash = md5.ComputeHash(Encoding.UTF8.GetBytes(emailAddress));
            return string.Concat(rawHash.Select(d => d.ToString("X2")));
        }
    }
}
