using System.Threading.Tasks;
using IdentityManagement.Areas.Api.Contracts.Queries;
using IdentityManagement.Areas.Api.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManagement.Areas.Api.Controllers
{
    [Authorize]
    [Area("api")]
    [ApiController, Route("[area]/[controller]")]
    public class RolesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginationList<Role>>> GetRolesAsync(
            [FromServices] RoleListQuery roleQuery,
            int? page = null,
            int? itemCount = null)
        {
            var items = await roleQuery.Paginate();
            return items;
        }
    }
}
