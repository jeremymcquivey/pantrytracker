using IdentityServer4.Models;
using System.Collections.Generic;

namespace PantryTracker.SingleSignOn.STS
{
    public class AppSettings
    {
        public ICollection<string> CorsOrigins { get; set; }

        public ICollection<Client> Clients { get; set; }
    }
}
