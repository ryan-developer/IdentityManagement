using System.Threading.Tasks;
using IdentityManagement.Areas.Api.Contracts.Requests;
using IdentityManagement.Areas.Api.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManagement.Areas.Api.Controllers
{
    [Authorize]
    [Area("api")]
    [ApiController, Route("[area]/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginationList<User>>> GetUsersAsync(
            [FromServices] UserListQuery usersRequest,
            int? page = null, 
            int? itemCount = null)
        {
            var users = await usersRequest.Paginate(page, itemCount);
            return users;
        }

        //[HttpGet]
        //public async Task<ActionResult<UserListResponse>> GetUsersAsync([FromServices] IUserListRequest usersRequest) 
        //{
        //    var users = await usersRequest.GetAllUsersAsync();
        //    return new UserListResponse {
        //        Users = users
        //    };
        //}

        //[HttpGet("{userId}")]
        //public async Task<ActionResult<UserResponse>> GetUserAsync([FromServices] IUserRequest userRequest)
        //{
        //    UserResponse user = await userRequest.GetResponse();
        //    return user;
        //}
    }
}
