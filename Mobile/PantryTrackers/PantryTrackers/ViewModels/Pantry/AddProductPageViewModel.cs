using System.Collections.ObjectModel;
using PantryTrackers.Models.Products;
using PantryTrackers.Services;
using Prism.Navigation;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.Pantry
{
    internal class AddProductPageViewModel: ViewModelBase
    {
        private const string EachQuantity = "Each/Count";
        private const string QuantityInUnits = "Quantity in Units";

        private Product _product;
        private Command _saveProductCommand;
        private ProductService _productService;
        private string _errorMessage;
        private ObservableCollection<string> _unitOptions;

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public ObservableCollection<string> UnitDisplayOptions => _unitOptions ??=
            new ObservableCollection<string>
            {
                EachQuantity,
                QuantityInUnits
            };

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

        public string SelectedUnitDisplay
        {
            get
            {
                switch(Product?.QuantityDisplayMode ?? ProductDisplayMode.EachUnit)
                {
                    case ProductDisplayMode.PurchaseQuantity:
                        return QuantityInUnits;
                    case ProductDisplayMode.EachUnit:
                    default:
                        return EachQuantity;
                }
            }
            set
            {
                if (Product != default)
                {
                    switch(value)
                    {
                        case QuantityInUnits:
                            Product.QuantityDisplayMode = ProductDisplayMode.EachUnit;
                            break;
                        case EachQuantity:
                        default:
                            Product.QuantityDisplayMode = ProductDisplayMode.PurchaseQuantity;
                            break;
                    }
                }
            }
        }

        public Product Product
        {
            get => _product;
            set
            {
                _product = value;
                RaisePropertyChanged(nameof(Product));
                RaisePropertyChanged(nameof(SelectedUnitDisplay));
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

            if(parameters.ContainsKey("SelectedProduct") && parameters["SelectedProduct"].GetType() == typeof(Product))
            {
                Product = (Product)parameters["SelectedProduct"];
            }
        }

        public override void OnCommandCanExecuteChanged()
        {
            base.OnCommandCanExecuteChanged();

            SaveProductCommand.ChangeCanExecute();
        }
    }
}
