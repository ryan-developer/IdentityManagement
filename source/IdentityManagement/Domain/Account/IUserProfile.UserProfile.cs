using IdentityManagement.Models;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IdentityManagement.Domain.Account
{
    public class UserProfile : IUserProfile
    {
        private readonly HttpContext context;

        public UserProfile(IHttpContextAccessor contextAccessor)
        {
            context = contextAccessor.HttpContext;

            ViewModel = new UserProfileViewModel();
            ViewModel.EmailAddress = context.User.FindFirstValue(JwtClaimTypes.Email);
            ViewModel.Name = context.User.FindFirstValue(JwtClaimTypes.Name);
            ViewModel.IsAuthenticated = context.User.Identity?.IsAuthenticated ?? false;

            string gravatarId = GenerateGravatarHash(ViewModel.EmailAddress);
            ViewModel.GravatarUrl = $"https://www.gravatar.com/avatar/{gravatarId}?d=mp";
        }

        public UserProfileViewModel ViewModel { get; private set; }

        private string GenerateGravatarHash(string emailAddress)
        {
            var md5 = MD5.Create();
            byte[] rawHash = md5.ComputeHash(Encoding.UTF8.GetBytes(emailAddress));
            return string.Concat(rawHash.Select(d => d.ToString("X2")));
        }
    }
}
