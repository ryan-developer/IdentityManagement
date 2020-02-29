using System.Linq;
using System.Threading.Tasks;
using IdentityManagement.Areas.Api.Contracts.Responses;
using IdentityManagement.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using CommandQuery;

namespace IdentityManagement.Areas.Api.Contracts.Queries
{
    public class RoleListQuery : IAsyncQuery
    {
        private readonly TenantUserDbContext dbContext;

        public RoleListQuery(TenantUserDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<PaginationList<Role>> Paginate()
        {
            var roles = await dbContext.Roles
            .Select(role => 
                new Role 
                {  
                    Name = role.NormalizedName,
                    Id = role.Id
                }).ToListAsync();

            return new PaginationList<Role> { 
                ItemCount = roles.Count,
                Page = 1,
                PageCount = 1,
                TotalCount = roles.Count,
                Items = roles
            };
        }
    }
}
