namespace IdentityManagement.Models
{
    public class UserProfileViewModel
    {
        public bool IsAuthenticated { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string GravatarUrl { get; set; }
    }
}
