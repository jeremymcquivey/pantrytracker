using System.Collections.Generic;

namespace PantryTracker.Model.Auth
{
    public class AuthContext
    {
        public IEnumerable<string> Roles { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}
