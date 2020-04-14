using Microsoft.AspNetCore.Identity;

namespace SecuringAngularApps.STS.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<string>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}