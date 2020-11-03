using PantryTrackers.Auth;
using System;
using Xamarin.Auth;
using System.Linq;
using System.Net.Http;
using Xamarin.Auth.Presenters;
using System.Threading.Tasks;
using PantryTrackers.Models;
using Microsoft.Extensions.Configuration;
using System.IO;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace PantryTrackers.Security
{
    class AuthenticationService
    {
        private const char RoleDelimiter = '|';
        private const string AuthContextEndpoint = "v1/User/AuthContext";
        private readonly IConfiguration _config;
        public readonly IHttpClientFactory _clientFactory;

        public event EventHandler SuccessfulAuthentication;
        public event EventHandler UnsuccessfulAuthentication;
        public event EventHandler SystemAuthenticationError;

        private static Account AuthAccount { get; set; }

        public AuthenticationService(IHttpClientFactory factory, IConfiguration config)
        {
            _config = config;
            _clientFactory = factory;
        }

        public async void Authenticate()
        {
            var oAuth = new OAuth2AuthenticatorEx("pantrytrackers-mobile", "pantrytrackers-api",
                new Uri("https://pantrytrackers-identity-dev.azurewebsites.net/connect/authorize"), new Uri("https://pantrytrackers-identity-dev.azurewebsites.net/redirect"))
            {
                AccessTokenUrl = new Uri("https://pantrytrackers-identity-dev.azurewebsites.net/connect/token"),
                ShouldEncounterOnPageLoading = false
            };

            var account = await GetUserProfile();
            if (account != null && DateTime.Now.ToUniversalTime() <= account.Expires)
            {
                using(var client = _clientFactory.CreateClient())
                { 
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AuthAccount.Properties["access_token"]}");
                }
            }
            else
            {
                var presenter = new OAuthLoginPresenter();
                presenter.Completed += Presenter_Completed;
                presenter.Login(oAuth);
            }
        }

        public async Task<bool> IsAuthenticated()
        {
            return false;
        }

        public static async Task<AuthContext> GetUserProfile()
        {
            long.TryParse(await SecureStorage.GetAsync(ClaimKeys.Expires), out long expTicks);
            return new AuthContext
            {
                Expires = new DateTime(expTicks),
                Roles = (await SecureStorage.GetAsync(ClaimKeys.Roles))?.Split(RoleDelimiter),
                UserProfile = new UserProfile
                {
                    Email = await SecureStorage.GetAsync(ClaimKeys.Email),
                    Id = await SecureStorage.GetAsync(ClaimKeys.Id),
                    FirstName = await SecureStorage.GetAsync(ClaimKeys.FirstName),
                    LastName = await SecureStorage.GetAsync(ClaimKeys.LastName)
                }
            };
        }

        private async void Presenter_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                AuthAccount = e.Account;

                var context = await GetAuthContext();

                if(!string.IsNullOrEmpty(context?.UserProfile?.Id))
                {
                    await SecureStorage.SetAsync(ClaimKeys.Roles, string.Join(RoleDelimiter, context.Roles ?? Enumerable.Empty<string>()));
                    await SecureStorage.SetAsync(ClaimKeys.Email, context.UserProfile.Email ?? "default@user");
                    await SecureStorage.SetAsync(ClaimKeys.Id, context.UserProfile.Id);
                    await SecureStorage.SetAsync(ClaimKeys.LastName, context.UserProfile.LastName ?? "User");
                    await SecureStorage.SetAsync(ClaimKeys.FirstName, context.UserProfile.FirstName ?? "Annonymous");
                    await SecureStorage.SetAsync(ClaimKeys.Expires, $"{context.Expires.Ticks}");

                    SuccessfulAuthentication.Invoke(this, new EventArgs());
                }
                else
                {
                    SystemAuthenticationError.Invoke(this, new EventArgs());
                }
            }
            else
            {
                UnsuccessfulAuthentication.Invoke(this, new EventArgs());
            }
        }

        public async Task<AuthContext> GetAuthContext()
        {
            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AuthAccount.Properties["access_token"]}");
                    var baseAddress = _config.GetValue<string>("Endpoints:APIUrl");
                    client.BaseAddress = new Uri(baseAddress);
                    var result = await client.GetAsync(Path.Combine(baseAddress, AuthContextEndpoint));

                    if (result.IsSuccessStatusCode)
                    {
                        var context = await result.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<AuthContext>(context);
                    }
                    else
                    {
                        //TODO: Return meaningful message here.
                    }
                }

                return default;
            }
            catch(Exception ex)
            {
                //TODO: Log ex.
                return default;
            }
        }
    }
}
