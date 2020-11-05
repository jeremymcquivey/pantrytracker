using PantryTrackers.Views.Pantry;
using Prism.Navigation;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.Pantry
{
    public class PantryMainPageViewModel: ViewModelBase
    {
        private Command _addInventoryCommand;
        private readonly INavigationService _navigationService;

        public Command AddInventoryCommand => _addInventoryCommand ??= new Command(async () =>
        {
            await _navigationService.NavigateAsync(nameof(AddPantryTransactionPage));
        });

        public PantryMainPageViewModel(INavigationService navigationService):
            base(navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
