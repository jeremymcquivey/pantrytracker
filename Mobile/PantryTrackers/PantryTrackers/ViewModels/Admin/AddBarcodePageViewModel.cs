using PantryTrackers.Controls;
using PantryTrackers.Models.Products;
using PantryTrackers.Services;
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
        private Command _saveCodeCommand;

        public bool CanEditCode => string.IsNullOrEmpty(Code?.Vendor);

        public ProductCode Code 
        { 
            get => _code; 
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
                await _navService.NavigateAsync(nameof(BarcodeScannerPage));
            });

        public Command SaveCodeCommand => _saveCodeCommand ??=
            new Command(async () =>
            {
                //Todo: validate form input.
                var newCode = await _products.Save(Code);

                if(newCode != default)
                {
                    Code = newCode;
                }

                //todo: else an error occurred.
            });

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("BarcodeScanResult") && parameters["BarcodeScanResult"].GetType() == typeof(string))
            {
                Task.Run(async () =>
                {
                    var searchText = (string)parameters["BarcodeScanResult"];
                    var result = await _products.Search(searchText);

                    if(result != default)
                    {
                        Code = result;
                    }
                    else
                    {
                        Code = new ProductCode
                        {
                            Code = searchText
                        };
                        // todo: show error message that code was not found.
                        return;
                    }
                });
            }
        }

        public AddBarcodePageViewModel(INavigationService navigationService, ProductService products) :
            base(navigationService, null)
        {
            _navService = navigationService;
            _products = products;
        }
    }
}
