using PantryTrackers.Controls;
using PantryTrackers.Models.GroceryList;
using PantryTrackers.Services;
using PantryTrackers.Views.Admin;
using Prism.Navigation;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.GroceryList
{
    public class AddGroceryListItemPageViewModel: ViewModelBase
    {
        private Command _addGroceryListItemPageViewModel;
        private Command<string> _saveGroceryItem;

        private ProductService _products;

        public GroceryListItem Item { get; } =
            new GroceryListItem();

        private Command<string> SaveGroceryItem => _saveGroceryItem ??= new Command<string>(async (searchText) =>
        {
            IsNetworkBusy = true;
            var result = await _products.Search(searchText);

            if (result != default)
            {
                Item.ProductId = result.ProductId;
                Item.Product = result.Product;
                Item.DisplayName = result.Product?.Name;
                Item.Unit = result.Product?.DefaultUnit;
                Item.Status = GroceryListItemStatus.Active;

                if(result.Product == default)
                {
                    Item.FreeformText = result.Description;
                }

                await NavigationService.GoBackAsync(new NavigationParameters
                {
                    { "NewGroceryListEntry", Item }
                });
            }
            else
            {
                await NavigationService.NavigateAsync(nameof(AddBarcodePage), new NavigationParameters
                {
                    { "ProductCode", result.Code },
                });
            }

            IsNetworkBusy = false;
        }, CanExecute);

        public Command LaunchBarcodeScannerCommand => _addGroceryListItemPageViewModel ??= new Command(async () =>
        {
            IsNetworkBusy = true;
            await NavigationService.NavigateAsync(nameof(BarcodeScannerPage));
            IsNetworkBusy = false;
        }, CanExecute);

        public AddGroceryListItemPageViewModel(INavigationService navigationService, ProductService products) :
            base(navigationService, null)
        {
            _products = products;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("BarcodeScanResult") && parameters["BarcodeScanResult"].GetType() == typeof(string))
            {
                if(SaveGroceryItem.CanExecute(null))
                {
                    SaveGroceryItem.Execute((string)parameters["BarcodeScanResult"]);
                }
            }
        }
    }
}
