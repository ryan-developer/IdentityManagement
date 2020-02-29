using System.Threading.Tasks;

namespace IdentityManagement.Domain.Account
{
    public interface IRedirectRequest
    {
        Task<string> GetReturnUrl();
    }
}
