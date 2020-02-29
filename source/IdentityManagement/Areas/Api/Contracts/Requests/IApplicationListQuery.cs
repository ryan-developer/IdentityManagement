using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityManagement.Areas.Api.Contracts.Requests
{
    public interface IApplicationListQuery
    {
        Task<List<Client>> InvokeAsync();
    }

    public class ApplicationListRequest
    {
        protected readonly ConfigurationDbContext configContext;

        public ApplicationListRequest(ConfigurationDbContext configContext)
        {
            this.configContext = configContext;
        }

        public async Task<List<Client>> InvokeAsync()
        {
            var clients = configContext.Clients
                .Include(d => d.Claims)
                .Include(d => d.AllowedScopes)
                .Where(d => d.Claims.Any(c => c.Type == "role" && c.Value == "Application"))
                    .Select(d => d.ToModel())
                .ToListAsync();

            return await clients;
        }
    }
}
