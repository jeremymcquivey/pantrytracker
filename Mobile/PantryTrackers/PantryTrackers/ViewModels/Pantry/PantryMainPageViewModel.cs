using PantryTrackers.Common.Security;
using PantryTrackers.Models;
using PantryTrackers.Models.Meta.Enums;
using PantryTrackers.Services;
using PantryTrackers.Views.Pantry;
using Prism.Navigation;
using System.Collections.Generic;
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
        private Command _refreshDataCommand;

        private readonly PantryService _pantry;

        public ObservableCollection<ProductGroup> ProductGroups { get; } =
            new ObservableCollection<ProductGroup>();

        public Command RefreshDataCommand => _refreshDataCommand ??= new Command(async () =>
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
                IsNetworkBusy = false;
                RepopulateData(data);
            });
        }, CanExecute);

        public Command AddItemToListCommand => _addItemToListCommand ??= new Command((myParam) =>
        {

        });

        public Command AddInventoryCommand => _addInventoryCommand ??= new Command(async () =>
        {
            IsNetworkBusy = true;
            await NavigationService.NavigateAsync(nameof(AddPantryTransactionPage));
            IsNetworkBusy = false;
        }, CanExecute);

        public Command RemoveInventoryCommand => _removeInventoryCommand ??= new Command(async () =>
        {
            IsNetworkBusy = true;
            var navParams = new NavigationParameters
            {
                { "TransactionType", TransactionTypes.Usage }
            };

            await NavigationService.NavigateAsync(nameof(AddPantryTransactionPage), navParams);
            IsNetworkBusy = false;
        }, CanExecute);

        public PantryMainPageViewModel(INavigationService navigationService, PantryService pantry):
            base(navigationService, null)
        {
            _pantry = pantry;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if(RefreshDataCommand.CanExecute(null))
            {
                RefreshDataCommand.Execute(null);
            }
        }
        
        public override void OnCommandCanExecuteChanged()
        {
            base.OnCommandCanExecuteChanged();

            AddInventoryCommand.ChangeCanExecute();
            RemoveInventoryCommand.ChangeCanExecute();
            RefreshDataCommand.ChangeCanExecute();
            AddItemToListCommand.ChangeCanExecute();
        }

        private void RepopulateData(IEnumerable<ProductGroup> data)
        {
            ProductGroups.Clear();
            if (data == null)
            {
                return;
            }

            foreach (var group in data)
            {
                ProductGroups.Add(group);
            }
        }
    }
}
