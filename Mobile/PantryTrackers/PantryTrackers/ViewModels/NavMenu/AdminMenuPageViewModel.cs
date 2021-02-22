using System.Collections.Generic;
using System.Threading.Tasks;
using PantryTrackers.Common.Security;
using PantryTrackers.Common.Security.Models;
using PantryTrackers.Models;
using PantryTrackers.Models.NavMenu;
using PantryTrackers.Models.Products;
using PantryTrackers.Models.Recipes;
using PantryTrackers.Services;
using PantryTrackers.Views.Pantry;
using Prism.Navigation;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.NavMenu
{
    public class AdminMenuPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navService;
        private Command<NavMenuItem> _doSomethingcommand;

        public UserProfile Account { private set; get; }

        public string[] Roles = new string[0];

        public Recipe Recipe { get; private set; }

        public IEnumerable<NavMenuItem> MenuItems => Models.Meta.MenuItems.Admin();

        public Command DoSomethingCommand => _doSomethingcommand ??= new Command<NavMenuItem>(async (item) =>
        {
            if (string.IsNullOrEmpty(item.NavigationPage))
            {
                return;
            }

            await _navService.NavigateAsync($"{item.NavigationPage}");
        });

        public AdminMenuPageViewModel(INavigationService navigationService, RestClient client)
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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if(parameters.ContainsKey("SelectedProduct") && parameters["SelectedProduct"].GetType() == typeof(Product))
            {
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    await _navService.NavigateAsync($"{nameof(AddProductPage)}", parameters);
                });
            }
        }
    }
}
