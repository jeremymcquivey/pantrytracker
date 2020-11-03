using System;
using System.Collections.Generic;

namespace PantryTracker.Model.Auth
{
    public class AuthContext
    {
        public DateTime Expires { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}
