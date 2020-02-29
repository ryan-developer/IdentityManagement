using System.Collections.Generic;

namespace IdentityManagement.Areas.Account.Contracts
{
    public class UserAuthResponse
    {
        public bool IsAuthenticated { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string GravatarUrl { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}

