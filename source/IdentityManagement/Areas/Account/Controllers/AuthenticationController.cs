using System.Security.Claims;
using System.Threading.Tasks;
using IdentityManagement.Domain.Account;
using IdentityManagement.Filters;
using IdentityManagement.Persistence.Identity.Entities;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManagement.Areas.Account.Controllers
{
    [Area("account")]
    [Route("[area]")]
    [SecurityHeaders]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class AuthenticationController : Controller
    {
        private readonly IIdentityServerInteractionService interactionService;
        private readonly SignInManager<TenantUserEntity> signInManager;
        private readonly IEventService eventService;

        public AuthenticationController(
            IIdentityServerInteractionService interactionService,
            SignInManager<TenantUserEntity> signInManager,
            IEventService eventService
            )
        {
            this.interactionService = interactionService;
            this.signInManager = signInManager;
            this.eventService = eventService;
        }

        [AllowAnonymous]
        [HttpPost("sign-out")]
        [HttpGet("sign-out")]
        public async Task<IActionResult> SignOut(string logoutId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction(nameof(SignIn), new { signedOut = true });
            }

            LogoutRequest logout = await interactionService.GetLogoutContextAsync(logoutId);
            await signInManager.SignOutAsync();
            return Redirect("~/account/sign-in");
        }

        [Authorize(AuthenticationSchemes = "aad, google")]
        [HttpGet("registration/openid/{provider}")]
        public async Task<IActionResult> ExternalSignIn(
            [FromRoute] string provider, 
            [FromServices] IOpenIdRequest openIdRequest,
            [FromServices] IRedirectRequest redirectRequest)
        {
            string returnUrl = await redirectRequest.GetReturnUrl();

            TenantUserEntity tenantUser = await openIdRequest.RegisterUser();
            await signInManager.SignInAsync(tenantUser, true, nameof(ExternalSignIn));
            ClaimsPrincipal principal = await signInManager.CreateUserPrincipalAsync(tenantUser);
            await signInManager.SignInAsync(tenantUser, false, nameof(ExternalSignIn));
            await HttpContext.SignInAsync(principal);
            
            await eventService.RaiseAsync(new UserLoginSuccessEvent(tenantUser.UserName, tenantUser.Id, tenantUser.UserName));

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("sign-in/openid/{provider}")]
        public IActionResult ExternalChallenge([FromRoute] string provider, string returnUrl)
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = $"/account/registration/openid/{provider}",
                Items = {
                    { "scheme", IdentityServerConstants.DefaultCookieAuthenticationScheme },
                    { "returnUrl", returnUrl },
                }
            };
            return Challenge(properties, provider);
        }
    }
}
