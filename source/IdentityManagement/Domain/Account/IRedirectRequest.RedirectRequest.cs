using IdentityManagement.Exceptions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityManagement.Domain.Account
{
    public class RedirectRequest : IRedirectRequest
    {
        private readonly IIdentityServerInteractionService interactionService;
        private readonly IClientStore clientStore;
        private readonly HttpContext httpContext;
        private IUrlHelper urlHelper;

        public RedirectRequest(
            IIdentityServerInteractionService interactionService,
            IClientStore clientStore,
            IHttpContextAccessor contextAccessor,
            IUrlHelper urlHelper)
        {
            this.interactionService = interactionService;
            this.clientStore = clientStore;
            this.httpContext = contextAccessor.HttpContext;
            this.urlHelper = urlHelper;
        }

        public async Task<string> GetReturnUrl()
        {
            bool hasReturnUrl = httpContext.Request.Query.TryGetValue("returnUrl", out var returnUrl);
            if(!hasReturnUrl)
            {
                return null;
            }
            else if(urlHelper.IsLocalUrl(returnUrl))
            {
                return returnUrl;
            }

            AuthorizationRequest authRequest = 
                await interactionService.GetAuthorizationContextAsync(returnUrl);
            Client client = await clientStore.FindEnabledClientByIdAsync(authRequest.ClientId);
            if (client != null)
            {
                return returnUrl;
            }

            throw new UnAuthorizedRedirectException();
        }
    }
}
