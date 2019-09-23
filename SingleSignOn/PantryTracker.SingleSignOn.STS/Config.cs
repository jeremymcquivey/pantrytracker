using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace PantryTracker.SingleSignOn.STS{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("pantrytrackers-ui", "Projects API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "pantrytrackers-ui",
                    ClientName = "PantryTracker Web App",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RequireConsent = false,

                    RedirectUris = { "http://localhost:4200/assets/oidc-login-redirect.html" },
                    PostLogoutRedirectUris = { "http://localhost:4200/?postLogout=true" },
                    AllowedCorsOrigins = { "http://localhost:4200/" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "pantrytrackers-ui"
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }
    }
}