using PantryTrackers.Common.Security;
using PantryTrackers.Common.Security.Models;
using PantryTrackers.Models.NavMenu;
using PantryTrackers.Models.Recipes;
using PantryTrackers.Services;
using Prism.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.NavMenu
{
    public class NavMenuPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navService;
        private Command<NavMenuItem> _selectMenuItemCommand;

        public UserProfile Account { private set; get; }

        public string[] Roles = new string[0];

        public Recipe Recipe { get; private set; }

        public IEnumerable<NavMenuItem> MenuItems => Models.Meta.MenuItems.ForRoles(Roles);

        public Command SelectMenuItemCommand => _selectMenuItemCommand ??= new Command<NavMenuItem>(async (item) =>
        {
            IsNetworkBusy = true;
            if(string.IsNullOrEmpty(item.NavigationPage))
            {
                return;
            }

            await _navService.NavigateAsync($"NavigationPage/{item.NavigationPage}");
            IsNetworkBusy = false;
        }, (menuItem) => CanExecute(menuItem));

        public NavMenuPageViewModel(INavigationService navigationService, RestClient client)
            : base(navigationService, client)
        {
            _navService = navigationService;
            Task.Run(async () => 
            {
                var authContext = await AuthenticationService.GetUserProfile();

                Roles = authContext?.Roles ?? new string[0];
                RaisePropertyChanged(nameof(MenuItems));

                Account = authContext?.UserProfile;
                RaisePropertyChanged(nameof(Account));
            });
        }

        public override void OnCommandCanExecuteChanged()
        {
            SelectMenuItemCommand.ChangeCanExecute();

            base.OnCommandCanExecuteChanged();
        }
    }
}
