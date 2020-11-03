namespace PantryTrackers.Models
{
    public class UserProfile
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public object[] UserPermissions { get; set; }
    }
}
