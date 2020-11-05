namespace PantryTrackers.Common.Security.Models
{
    public class AuthContext
    {
        public string BearerToken { get; set; }

        public System.DateTime Expires { get; set; }

        public string[] Roles { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}
