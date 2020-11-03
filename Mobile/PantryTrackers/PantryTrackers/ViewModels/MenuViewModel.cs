using PantryTrackers.Models;
using PantryTrackers.Security;
using PantryTrackers.Views.Recipes;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace PantryTrackers.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private INavigationService _navService;

        public UserProfile Account { private set; get; }

        public Recipe Recipe { get; private set; }

        internal IEnumerable<MenuItem> MenuItems => Models.Meta.MenuItems.ForRoles(new List<string> { "Admin", "Premium" });

        public MenuViewModel(INavigationService navigationService)
            : base(navigationService) 
        {
            _navService = navigationService;
            Task.Run(async () => { Account = (await AuthenticationService.GetUserProfile()).UserProfile; RaisePropertyChanged(nameof(Account)); });
        }

        public async void DoSomething() 
        {
            await _navService.NavigateAsync(nameof(RecipeListPage));
        }
    }
}
