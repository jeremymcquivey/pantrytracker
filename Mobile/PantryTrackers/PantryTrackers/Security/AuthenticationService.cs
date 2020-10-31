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
using Xamarin.Forms;
using Newtonsoft.Json;

namespace PantryTrackers.Security
{
    class AuthenticationService
    {
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

        public void Authenticate()
        {
            var oAuth = new OAuth2AuthenticatorEx("pantrytrackers-mobile", "pantrytrackers-api",
                new Uri("https://pantrytrackers-identity-dev.azurewebsites.net/connect/authorize"), new Uri("https://pantrytrackers-identity-dev.azurewebsites.net/redirect"))
            {
                AccessTokenUrl = new Uri("https://pantrytrackers-identity-dev.azurewebsites.net/connect/token"),
                ShouldEncounterOnPageLoading = false
            };

            var account = AccountStore.Create().FindAccountsForService("AuthServer");
            if (account != null && account.Any())
            //if(false)
            {
                using(var client = _clientFactory.CreateClient())
                { 
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {account.First().Properties["access_token"]}");
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

        private async void Presenter_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                AuthAccount = e.Account;

                var context = await GetAuthContext();

                if(!string.IsNullOrEmpty(context?.UserProfile?.Id))
                {
                    AuthAccount.Properties.Add(nameof(UserProfile.FirstName).ToLower(), context.UserProfile.FirstName ?? "Annonymous");
                    AuthAccount.Properties.Add(nameof(UserProfile.LastName).ToLower(), context.UserProfile.LastName ?? "User");
                    AuthAccount.Properties.Add(nameof(UserProfile.Id).ToLower(), context.UserProfile.Id);
                    AuthAccount.Properties.Add(nameof(UserProfile.Email).ToLower(), context.UserProfile.Email ?? "default@user");

                    await AccountStore.Create().SaveAsync(e.Account, "AuthServer");
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
