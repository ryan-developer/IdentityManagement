using System.Collections.Generic;

namespace IdentityManagement.Areas.Api.ViewModels
{
    public enum AuthenticationType
    {
        ClientCredentials,
        InteractiveWeb,
        UserPassword
    }

    public class ApplicationRegistration
    {
        public string Name { get; set; }

        public List<string> ValidRedirectUrls { get; set; }

        public AuthenticationType ApplicationType { get; set; }

        public List<string> Scopes { get; set; }

        public bool RequireConsent { get; set; }
    }
}
