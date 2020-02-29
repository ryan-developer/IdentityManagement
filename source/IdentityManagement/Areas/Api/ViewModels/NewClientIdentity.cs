namespace IdentityManagement.Areas.Api.ViewModels
{
    public class NewClientIdentity
    {
        public string ClientId { get; set; }

        public string PrimarySecret { get; set; }

        public string SecondarySecret { get; set; }
    }
}
