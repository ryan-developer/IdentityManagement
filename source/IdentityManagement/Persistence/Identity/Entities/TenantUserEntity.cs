using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityManagement.Persistence.Identity.Entities
{
    public class TenantUserEntity : IdentityUser
    {
        [PersonalData]
        [MaxLength(500)]
        public string TenantID { get; set; }

        [PersonalData]
        [MaxLength(500)]
        public string IdentitySource { get; set; }

        [PersonalData]
        [MaxLength(500)]
        public string IdentityProvider { get; set; }

        [PersonalData]
        [MaxLength(500)]
        public string ExternalUserId { get; set; }


        public virtual List<IdentityUserClaim<string>> Claims { get; set; }

        public virtual List<IdentityUserRole<string>> Roles { get; set; }
    }
}