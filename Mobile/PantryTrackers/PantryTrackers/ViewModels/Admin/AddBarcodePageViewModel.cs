using PantryTrackers.Controls;
using PantryTrackers.Models;
using PantryTrackers.Models.Products;
using PantryTrackers.Services;
using PantryTrackers.Views.Pantry;
using Prism.Navigation;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.Admin
{
    class AddBarcodePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navService;
        private readonly ProductService _products;

        private ProductCode _code;
        private Command _launchBarcodeScannerCommand;
        private Command _editProductCommand;
        private Command _saveCodeCommand;
        private Command _clearCommand;
        private bool _showMessage;

        public bool CanEditCode => string.IsNullOrEmpty(Code?.Vendor);

        public bool ShowMessage
        {
            get => _showMessage;
            set
            {
                _showMessage = value;
                RaisePropertyChanged(nameof(ShowMessage));
            }
        }

        public ProductCode Code
        { 
            get => _code ??= new ProductCode();
            private set
            {
                _code = value;
                RaisePropertyChanged(nameof(Code));
                RaisePropertyChanged(nameof(CanEditCode));
            } 
        }

        public Command LaunchBarcodeScannerCommand => _launchBarcodeScannerCommand ??=
            new Command(async () =>
            {
                IsNetworkBusy = true;
                await _navService.NavigateAsync(nameof(BarcodeScannerPage));
                IsNetworkBusy = false;
            }, CanExecute);

        public Command SaveCodeCommand => _saveCodeCommand ??=
            new Command(async () =>
            {
                IsNetworkBusy = true;
                //Todo: validate form input.
                var newCode = await _products.SaveCode(Code);

                if(newCode != default)
                {
                    var product = Code.Product;
                    Code = newCode;
                    newCode.Product = product;

                    await _navService.GoBackAsync(new NavigationParameters
                    {
                        { "AddedCode", newCode }
                    });
                }
                IsNetworkBusy = false;
                //todo: else an error occurred.
            }, CanExecute);

        public Command ClearCommand => _clearCommand ??=
            new Command(() =>
            {
                IsNetworkBusy = true;
                Code = new ProductCode();
                IsNetworkBusy = false;
            }, CanExecute);

        public Command EditProductCommand => _editProductCommand ??=
            new Command(async () =>
            {
                IsNetworkBusy = true;
                var navParams = new NavigationParameters
                {
                    { "SelectedProduct", Code?.Product }
                };

                await _navService.NavigateAsync(nameof(ProductSearchPage));
                IsNetworkBusy = false;
            }, CanExecute);

        public AddBarcodePageViewModel(INavigationService navigationService, ProductService products) :
            base(navigationService, null)
        {
            _navService = navigationService;
            _products = products;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if(parameters.ContainsKey("ProductCode") && parameters["ProductCode"].GetType() == typeof(string))
            {
                Code = new ProductCode
                {
                    Code = (string)parameters["ProductCode"]
                };
            }

            if (parameters.ContainsKey("BarcodeScanResult") && parameters["BarcodeScanResult"].GetType() == typeof(string))
            {
                ShowMessage = false;
                IsNetworkBusy = true;

                Task.Run(async () =>
                {
                    var searchText = (string)parameters["BarcodeScanResult"];
                    var result = await _products.Search(searchText);

                    if(result != default)
                    {
                        Code = result;
                        await Device.InvokeOnMainThreadAsync(() =>
                        {
                            IsNetworkBusy = false;
                        });
                    }
                    else
                    {
                        Code = new ProductCode
                        {
                            Code = searchText
                        };

                        await Device.InvokeOnMainThreadAsync(() =>
                        {
                            ShowMessage = true;
                            IsNetworkBusy = false;
                        });
                        return;
                    }
                });
            }

            if (parameters.ContainsKey("SelectedProduct") && parameters["SelectedProduct"].GetType() == typeof(Product))
            {
                Code.Product = (Product)parameters["SelectedProduct"];
                Code.ProductId = Code.Product?.Id ?? 0;

                RaisePropertyChanged(nameof(Code));
            }
        }

        public override void OnCommandCanExecuteChanged()
        {
            base.OnCommandCanExecuteChanged();

            ClearCommand.ChangeCanExecute();
            LaunchBarcodeScannerCommand.ChangeCanExecute();
            EditProductCommand.ChangeCanExecute();
            SaveCodeCommand.ChangeCanExecute();
        }
    }
}
