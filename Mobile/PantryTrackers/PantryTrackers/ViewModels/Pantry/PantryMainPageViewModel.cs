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

        public PantryMainPageViewModel(INavigationService navigationService, RestClient client):
            base(navigationService, client)
        {
            _navigationService = navigationService;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            // TODO: Eventually, when we support multiple pantries, this will be the pantry requested.
            // For now, though, this is the user Id.
            var pantryId = await SecureStorage.GetAsync(ClaimKeys.Id);
            var url = $"v1/Pantry/{pantryId}?includeZeroValues=false";
            var response = await Client.MakeRequest<object>(new Uri(url, UriKind.Relative), HttpMethod.Get, isSecure: true);
            var data = await response.GetDeserializedContent<IEnumerable<ProductGroup>>();

            await Device.InvokeOnMainThreadAsync(() =>
            {
                ProductGroups.Clear();
                foreach (var group in data)
                {
                    ProductGroups.Add(group);
                }
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
