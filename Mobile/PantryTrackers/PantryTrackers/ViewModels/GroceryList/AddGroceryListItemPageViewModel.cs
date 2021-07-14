using System.Linq;
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
        private Command _saveListItemCommand;
        private Command<string> _productFromCodeCommand;
        private Command _productFromTextCommand;

        private ProductService _products;
        private string _searchText;

        public GroceryListItem Item { get; private set; } =
            new GroceryListItem { Quantity = 1 };

        public string SearchText
        {
            get => _searchText;
            set {
                _searchText = value;
                RaisePropertyChanged(nameof(SearchText));
            }
        }

        private Command<string> ProductFromCodeCommand => _productFromCodeCommand ??= new Command<string>(async (searchText) =>
        {
            IsNetworkBusy = true;
            var result = await _products.Search(searchText);

            if (result != default)
            {
                SearchText = result.Product?.Name ?? string.Empty;
                Item.ProductId = result.ProductId;
                Item.Product = result.Product;
                Item.DisplayName = result.Product?.Name;
                Item.Unit = result.Unit ?? result.Product?.DefaultUnit;
                Item.Status = GroceryListItemStatus.Active;
                if(decimal.TryParse(result.Size, out decimal sizeVal))
                {
                    Item.Size = sizeVal;
                }

                RaisePropertyChanged(nameof(Item));

                if(result.Product == default)
                {
                    Item.FreeformText = result.Description;

                    await NavigationService.GoBackAsync(new NavigationParameters
                    {
                        { "NewGroceryListEntry", Item }
                    });
                }
            }
            else
            {
                await NavigationService.NavigateAsync(nameof(AddBarcodePage), new NavigationParameters
                {
                    { "ProductCode", searchText },
                });
            }

            IsNetworkBusy = false;
        }, CanExecute);

        public Command ProductFromTextSearch => _productFromTextCommand ??= new Command(async () =>
        {
            if(string.IsNullOrEmpty(SearchText))
            {
                // todo: validation error;
                return;
            }

            IsNetworkBusy = true;
            var results = await _products.SearchByText(SearchText);

            if (results != default && results.Any())
            {
                var result = results.First();
                Item = new GroceryListItem
                {
                    Quantity = 1,
                    ProductId = result.Id,
                    Product = result,
                    DisplayName = result.Name,
                    Unit = result.DefaultUnit,
                    Status = GroceryListItemStatus.Active
                };

                SearchText = string.Empty;
                RaisePropertyChanged(nameof(Item));
            }
            else
            {
                Item = new GroceryListItem
                {
                    Quantity = 1,
                    FreeformText = SearchText,
                    DisplayName = SearchText
                };

                await NavigationService.GoBackAsync(new NavigationParameters
                {
                    { "NewGroceryListEntry", Item }
                });
            }

            IsNetworkBusy = false;
        }, CanExecute);

        public Command SaveListItemCommand => _saveListItemCommand ??= new Command(async () =>
        {
            if(Item.Product != default)
            {
                Item.FreeformText = null;
                RaisePropertyChanged(nameof(Item));
            }

            if(!Item.IsFreeform && Item.Product == default)
            {
                // todo: log some sort of validation error here.
                return;
            }

            IsNetworkBusy = true;
            await NavigationService.GoBackAsync(new NavigationParameters
            {
                { "NewGroceryListEntry", Item }
            });
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
                if(ProductFromCodeCommand.CanExecute(null))
                {
                    ProductFromCodeCommand.Execute((string)parameters["BarcodeScanResult"]);
                }
            }
        }

        public override void OnCommandCanExecuteChanged()
        {
            base.OnCommandCanExecuteChanged();

            LaunchBarcodeScannerCommand.ChangeCanExecute();
            ProductFromCodeCommand.ChangeCanExecute();
            SaveListItemCommand.ChangeCanExecute();
        }
    }
}
