using PantryTrackers.Common.Extensions;
using PantryTrackers.Common.Security;
using PantryTrackers.Models;
using PantryTrackers.Services;
using PantryTrackers.Views.Pantry;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.Pantry
{
    public class PantryMainPageViewModel : ViewModelBase
    {
        private Command _addInventoryCommand;
        private Command _removeInventoryCommand;
        private readonly PantryService _pantry;
        private readonly INavigationService _navigationService;

        public Command SomeCommand => new Command(() =>
        {
            //ProductGroups = new List<ProductGroup>();
        });

        public ObservableCollection<ProductGroup> ProductGroups { get; } =
            new ObservableCollection<ProductGroup>();

        public Command AddInventoryCommand => _addInventoryCommand ??= new Command(async () =>
        {
            IsNetworkBusy = true;
            await _navigationService.NavigateAsync(nameof(AddPantryTransactionPage));
            IsNetworkBusy = false;
        }, CanExecute);

        public Command RemoveInventoryCommand => _removeInventoryCommand ??= new Command(async () =>
        {
            IsNetworkBusy = false;
            await Task.Run(() => { });
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
        }

        public override void OnCommandCanExecuteChanged()
        {
            base.OnCommandCanExecuteChanged();

            AddInventoryCommand.ChangeCanExecute();
            RemoveInventoryCommand.ChangeCanExecute();
        }
    }
}
