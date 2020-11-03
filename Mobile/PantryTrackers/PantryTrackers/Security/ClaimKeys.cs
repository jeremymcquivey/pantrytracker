using PantryTrackers.Models;

namespace PantryTrackers.Security
{
    internal static class ClaimKeys
    {
        internal static string Roles = "Roles";

        internal static string Email = nameof(UserProfile.Email);

        internal static string Id = nameof(UserProfile.Id);

        internal static string Expires = nameof(AuthContext.Expires);

        internal static string FirstName = nameof(UserProfile.FirstName);

        internal static string LastName = nameof(UserProfile.LastName);
    }
}
