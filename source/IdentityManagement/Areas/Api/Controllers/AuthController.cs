using IdentityManagement.Areas.Account.Contracts;
using IdentityManagement.Areas.Api.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManagement.Areas.Account.Controllers
{
    [Area("api")]
    [ApiController]
    [Route("[area]/auth")]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        public ActionResult<UserAuthResponse> GetAuthenticatedUser([FromServices] IAuthenticatedUserRequest userRequest)
            => userRequest.Invoke();
    }
}