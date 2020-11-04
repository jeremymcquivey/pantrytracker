using PantryTrackers.Models;
using PantryTrackers.Security;
using Prism.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly INavigationService _navService;
        private Command<Models.MenuItem> _doSomethingcommand;

        public UserProfile Account { private set; get; }

        public Recipe Recipe { get; private set; }

        internal IEnumerable<Models.MenuItem> MenuItems => Models.Meta.MenuItems.ForRoles(new List<string> { "Admin", "Premium" });

        public Command DoSomethingCommand => _doSomethingcommand ??= new Command<Models.MenuItem>(async (item) =>
        {
            await _navService.NavigateAsync(item.NavigationPage);
        });

        public MenuViewModel(INavigationService navigationService)
            : base(navigationService) 
        {
            _navService = navigationService;
            Task.Run(async () => { Account = (await AuthenticationService.GetUserProfile()).UserProfile; RaisePropertyChanged(nameof(Account)); });
        }
    }
}
