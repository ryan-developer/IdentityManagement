using System.Collections.Generic;

namespace IdentityManagement.Areas.Api.ViewModels
{
    public class UserListItem
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string TenantId { get; set; }

        public string IdentityProvider { get; set; }

        public string Id { get; set; }

        public string ExternalId { get; set; }

        public IEnumerable<ClaimListItem> Claims { get; set; }
    }
}
