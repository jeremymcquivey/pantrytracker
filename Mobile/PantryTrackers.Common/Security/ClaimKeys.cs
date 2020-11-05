using PantryTrackers.Common.Security.Models;

namespace PantryTrackers.Common.Security
{
    public static class ClaimKeys
    {
        public static string Roles = "Roles";

        public static string Email = nameof(UserProfile.Email);

        public static string Id = nameof(UserProfile.Id);

        public static string Expires = nameof(AuthContext.Expires);

        public static string FirstName = nameof(UserProfile.FirstName);

        public static string LastName = nameof(UserProfile.LastName);

        public static string BearerToken = "BearerToken";
    }
}
