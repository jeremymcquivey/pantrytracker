using IdentityServer4.Models;
using System.Collections.Generic;

namespace PantryTrackers.STS
{
    public class AppSettings
    {
        public ICollection<ApiResource> Apis { get; set; }

        public ICollection<string> CorsOrigins { get; set; }

        public ICollection<Client> Clients { get; set; }
    }
}
