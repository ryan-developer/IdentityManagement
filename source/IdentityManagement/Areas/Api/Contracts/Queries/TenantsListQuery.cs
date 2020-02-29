using System.Linq;
using System.Threading.Tasks;
using IdentityManagement.Areas.Api.Contracts.Responses;
using IdentityManagement.Persistence.Tenancy;
using Microsoft.EntityFrameworkCore;

namespace IdentityManagement.Areas.Api.Contracts.Queries
{
    public class TenantsListQuery
    {
        private readonly TenantDirectoryDbContext dbContext;

        public TenantsListQuery(TenantDirectoryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<PaginationList<Tenant>> Paginate()
        {
            var tenants = await dbContext.Tenants.Select(
                d => new Tenant { 
                    Id = d.Id.ToString(),
                    Name = d.CompanyName
                }).ToListAsync();

            return new PaginationList<Tenant> { 
                Items = tenants,
                ItemCount = tenants.Count,
                Page = 1, 
                PageCount = 1,
                TotalCount = tenants.Count
            };
        }
    }
}
