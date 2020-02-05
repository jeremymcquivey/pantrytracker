using System;
using System.Collections.Generic;
using System.Text;

namespace PantryTrackers.Integrations.Kroger
{
    public class KrogerAuthToken
    {
        public int expires_in { get; set; }

        public string access_token { get; set; }

        public string token_type { get; set; }
    }
}
