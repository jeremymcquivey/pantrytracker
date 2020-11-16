using PantryTrackers.Views.Pantry;
using Prism.Navigation;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.Pantry
{
    public class PantryMainPageViewModel: ViewModelBase
    {
        private Command _addInventoryCommand;
        private readonly INavigationService _navigationService;

        public Command SomeCommand => new Command(() =>
        {
            PantryLines = new List<object> { new { }, new { }, new { } };
        });

        public IEnumerable<object> PantryLines { get; private set; } = new List<object> { new { Name = "Old 1" }, new { Name = "Old 2" } };

        public Command AddInventoryCommand => _addInventoryCommand ??= new Command(async () =>
        {
            await _navigationService.NavigateAsync(nameof(AddPantryTransactionPage));
        });

        public PantryMainPageViewModel(INavigationService navigationService):
            base(navigationService, null)
        {
            _navigationService = navigationService;
        }
    }
}
