using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Auth;

namespace PantryTrackers.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IEnumerable<Account> accounts = null;

        public string Name => accounts?.FirstOrDefault()?.Properties["access_token"] ?? "Annonymous";

        public string Refresh => accounts?.FirstOrDefault()?.Properties["refresh_token"] ?? "Annonymous";

        public MainPageViewModel(INavigationService navigationService) 
            : base (navigationService)
        {
            Title = "Main Page";

            accounts = AccountStore.Create().FindAccountsForService("AuthServer");
            RaisePropertyChanged(nameof(Name));
        }
    }
}