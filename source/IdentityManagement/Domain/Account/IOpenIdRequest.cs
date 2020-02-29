using System.Threading.Tasks;
using IdentityManagement.Persistence.Identity.Entities;

namespace IdentityManagement.Domain.Account
{
    public interface IOpenIdRequest
    {
        string ExternalUserId { get; }

        Task<TenantUserEntity> RegisterUser();
    }
}
