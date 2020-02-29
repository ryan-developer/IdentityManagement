using System.Threading.Tasks;
using IdentityManagement.Areas.Api.Contracts.Queries;
using IdentityManagement.Areas.Api.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManagement.Areas.Api.Controllers
{
    [Area("api")]
    [Route("[area]/[controller]")]
    [Authorize, ApiController]
    public class ApplicationsController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginationList<ClientApplication>>> GetAll(
            [FromServices] ApplicationListQuery query)
        {
            return await query.Paginate();
        }
    }
}
