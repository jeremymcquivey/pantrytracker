using PantryTrackers.Common.Security;
using PantryTrackers.Common.Security.Models;
using PantryTrackers.Models.NavMenu;
using PantryTrackers.Models.Recipes;
using Prism.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.NavMenu
{
    public class NavMenuPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navService;
        private Command<NavMenuItem> _doSomethingcommand;

        public UserProfile Account { private set; get; }

        public Recipe Recipe { get; private set; }

        public IEnumerable<NavMenuItem> MenuItems => Models.Meta.MenuItems.ForRoles(new List<string> { "Admin", "Premium" });

        public Command DoSomethingCommand => _doSomethingcommand ??= new Command<NavMenuItem>(async (item) =>
        {
            if(string.IsNullOrEmpty(item.NavigationPage))
            {
                return;
            }

            await _navService.NavigateAsync(item.NavigationPage);
        });

        public NavMenuPageViewModel(INavigationService navigationService)
            : base(navigationService) 
        {
            _navService = navigationService;
            Task.Run(async () => { Account = (await AuthenticationService.GetUserProfile()).UserProfile; RaisePropertyChanged(nameof(Account)); });
        }
    }
}
