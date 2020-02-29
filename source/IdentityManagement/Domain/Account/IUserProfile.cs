using IdentityManagement.Models;

namespace IdentityManagement.Domain.Account
{
    public interface IUserProfile
    {
        UserProfileViewModel ViewModel { get; }
    }
}