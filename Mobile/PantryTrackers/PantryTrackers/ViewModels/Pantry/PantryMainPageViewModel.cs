using PantryTrackers.Common.Security;
using PantryTrackers.Models;
using PantryTrackers.Models.Meta.Enums;
using PantryTrackers.Services;
using PantryTrackers.Views.Pantry;
using Prism.Navigation;
using System.Collections.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.Pantry
{
    public class PantryMainPageViewModel : ViewModelBase
    {
        private Command _addInventoryCommand;
        private Command _removeInventoryCommand;
        private Command _addItemToListCommand;
        private Command _someCommand;

        private readonly PantryService _pantry;
        private readonly INavigationService _navigationService;

        public Command SomeCommand => _someCommand ??= new Command(async () =>
        {
            // TODO: Eventually, when we support multiple pantries, this will be the pantry requested.
            // For now, though, this is the user Id.
            var pantryId = await SecureStorage.GetAsync(ClaimKeys.Id);

            await Device.InvokeOnMainThreadAsync(() =>
            {
                IsNetworkBusy = true;
            });

            var data = await _pantry.GetSummary(pantryId);

            await Device.InvokeOnMainThreadAsync(() =>
            {
                ProductGroups.Clear();
                foreach (var group in data)
                {
                    ProductGroups.Add(group);
                }
                IsNetworkBusy = false;
            });
        }, CanExecute);

        public ObservableCollection<ProductGroup> ProductGroups { get; } =
            new ObservableCollection<ProductGroup>();

        public Command AddItemToListCommand => _addItemToListCommand ??= new Command((myParam) =>
        {
            SomeCommand.Execute(null);
        });

        public Command AddInventoryCommand => _addInventoryCommand ??= new Command(async () =>
        {
            IsNetworkBusy = true;
            await _navigationService.NavigateAsync(nameof(AddPantryTransactionPage));
            IsNetworkBusy = false;
        }, CanExecute);

        public Command RemoveInventoryCommand => _removeInventoryCommand ??= new Command(async () =>
        {
            IsNetworkBusy = true;
            var navParams = new NavigationParameters
            {
                { "TransactionType", TransactionTypes.Usage }
            };

            await _navigationService.NavigateAsync(nameof(AddPantryTransactionPage), navParams);
            IsNetworkBusy = false;
        }, CanExecute);

        public PantryMainPageViewModel(INavigationService navigationService, PantryService pantry):
            base(navigationService, null)
        {
            _pantry = pantry;
            _navigationService = navigationService;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            SomeCommand.Execute(null);
        }

        public override void OnCommandCanExecuteChanged()
        {
            base.OnCommandCanExecuteChanged();

            AddInventoryCommand.ChangeCanExecute();
            RemoveInventoryCommand.ChangeCanExecute();
            SomeCommand.ChangeCanExecute();
            AddItemToListCommand.ChangeCanExecute();
        }
    }
}
