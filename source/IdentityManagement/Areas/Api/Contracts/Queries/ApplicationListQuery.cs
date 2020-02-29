using System.Linq;
using System.Threading.Tasks;
using IdentityManagement.Areas.Api.Contracts.Responses;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using CommandQuery;

namespace IdentityManagement.Areas.Api.Contracts.Queries
{
    public class ApplicationListQuery : IAsyncQuery
    {
        protected readonly ConfigurationDbContext configContext;

        public ApplicationListQuery(ConfigurationDbContext configContext)
        {
            this.configContext = configContext;
        }

        public async Task<PaginationList<ClientApplication>> Paginate()
        {
            var clients = await configContext.Clients
                .Include(d => d.Claims)
                .Include(d => d.AllowedScopes)
                .Include(d => d.AllowedGrantTypes)
                .Include(d => d.PostLogoutRedirectUris)
                .Include(d => d.RedirectUris)
                .Select(d => new ClientApplication { 
                    Name = d.ClientName
                })
                .ToListAsync();

            return new PaginationList<ClientApplication>()
            {
                Items = clients
            };
        }
    }
}
