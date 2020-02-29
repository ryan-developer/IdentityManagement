using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityManagement.Areas.Api.Contracts.Responses;
using IdentityManagement.Persistence.Identity;
using IdentityManagement.Persistence.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CommandQuery;

namespace IdentityManagement.Areas.Api.Contracts.Requests
{
    public class UserListQuery : IAsyncQuery
    {
        private const int DEFAULT_ITEM_COUNT = 100;

        private readonly TenantUserDbContext dbContext;
        private readonly UserManager<TenantUserEntity> userManager;

        public UserListQuery(
            TenantUserDbContext dbContext,
            UserManager<TenantUserEntity> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<PaginationList<User>> Paginate(int? page = null, int? itemCount = null)
        {
            int totalUsers = dbContext.Users.Count();
            int requestedPage = page.HasValue && page > 0 ? page.Value : 1;
            int requestedItemCount = itemCount.HasValue && itemCount > 0 ? itemCount.Value : DEFAULT_ITEM_COUNT;

            decimal pageCount = totalUsers <= requestedItemCount ? 1 : totalUsers / requestedItemCount;
            int roundedPageCount = (int)Math.Round(pageCount, MidpointRounding.AwayFromZero);

            int skipCount = (requestedPage - 1) * requestedItemCount;

            var userEntities =
                dbContext.Users
                .Include(d => d.Claims)
                .Include(d => d.Roles)
                .OrderBy(d => d.Id)
                .Skip(skipCount)
                .Take(requestedItemCount)
                .Select(d => new User()
                {
                    Id = d.Id,
                    Email = d.Email.ToLower(),
                    TenantID = d.TenantID,
                    IdentityClaims = d.Claims,
                    IdentityRoles = d.Roles
                });

            List<User> users = await userEntities.ToListAsync();
            return new PaginationList<User>
            {
                TotalCount = totalUsers,
                Page = requestedPage,
                PageCount = roundedPageCount,
                ItemCount = users.Count,
                Items = users
            };
        }
    }
}