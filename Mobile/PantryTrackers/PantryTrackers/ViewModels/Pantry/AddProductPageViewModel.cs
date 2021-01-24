using PantryTrackers.Models;
using PantryTrackers.Services;
using Prism.Navigation;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.Pantry
{
    internal class AddProductPageViewModel: ViewModelBase
    {
        private Product _product;
        private Command _saveProductCommand;
        private ProductService _productService;
        private string _errorMessage;

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public string ErrorMessage
        {
            get => _errorMessage;
            private set
            {
                _errorMessage = value;
                RaisePropertyChanged(nameof(ErrorMessage));
                RaisePropertyChanged(nameof(HasErrorMessage));
            }
        }

        public Product Product
        {
            get => _product;
            set
            {
                _product = value;
                RaisePropertyChanged(nameof(Product));
            }
        }

        public Command SaveProductCommand => _saveProductCommand ??=
            new Command(async () =>
            {
                IsNetworkBusy = true;
                ErrorMessage = string.Empty;

                var newProduct = await _productService.Save(Product);
                if(newProduct != default)
                {
                    var navParams = new NavigationParameters
                    {
                        { "NewProduct", newProduct }
                    };

                    IsNetworkBusy = false;
                    await NavigationService.GoBackAsync(navParams);
                    return;
                }

                IsNetworkBusy = false;
                ErrorMessage = "Product did not save.";
            }, CanExecute);

        public AddProductPageViewModel(INavigationService navigationService,
                                       ProductService productService,
                                       RestClient restClient)
            : base(navigationService, restClient)
        {
            _productService = productService;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Product = new Product
            {
                Name = parameters.ContainsKey("ProductName") ? (string)parameters["ProductName"] : string.Empty
            };
        }

        public override void OnCommandCanExecuteChanged()
        {
            base.OnCommandCanExecuteChanged();

            SaveProductCommand.ChangeCanExecute();
        }
    }
}
