using System.Threading.Tasks;
using PantryTrackers.Controls;
using PantryTrackers.Models;
using PantryTrackers.Services;
using Prism.Navigation;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.Pantry
{
    internal class AddPantryTransactionPageViewModel : ViewModelBase
    {
        private PantryTransaction _transaction;
        private Command _launchBarcodeScannerCommand;
        private Command _saveTransactionCommand;
        private string _warningMessage;
        private readonly INavigationService _navService;
        private readonly ProductService _products;

        public Command LaunchBarcodeScannerCommand => _launchBarcodeScannerCommand ??=
            new Command(async () => 
            { 
                await _navService.NavigateAsync(nameof(BarcodeScannerPage)); 
            });

        public Command SaveTransactionCommand => _saveTransactionCommand ??=
            new Command(async () =>
            {
                // Save Transaction
                await _navService.GoBackAsync(new NavigationParameters
                {
                    { "NewTransaction", Transaction }
                });
            });

        public string WarningMessage
        {
            get => _warningMessage;
            private set {
                _warningMessage = value;
                RaisePropertyChanged(nameof(WarningMessage));
                RaisePropertyChanged(nameof(HasWarningMessage));
            }
        }

        public bool HasWarningMessage => !string.IsNullOrEmpty(WarningMessage);

        public PantryTransaction Transaction 
        { 
            get => _transaction; 
            set 
            { 
                _transaction = value; 
                RaisePropertyChanged(nameof(Transaction)); 
            } 
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if(parameters.ContainsKey("BarcodeScanResult"))
            {
                WarningMessage = string.Empty;
                await Task.Run(async () =>
                {
                    var product = await _products.Search((string)parameters["BarcodeScanResult"]);
                    if (product != default)
                    {
                        Transaction = new PantryTransaction
                        {
                            ProductCode = (string)parameters["BarcodeScanResult"],
                            ContainerSize = product.Size,
                            Unit = product.Unit,
                            ProductId = product.Product?.Id,
                            ProductName = product.Product?.Name
                        };
                    }
                    else
                    {
                        Transaction = new PantryTransaction
                        {
                            ProductCode = (string)parameters["BarcodeScanResult"]
                        };

                        WarningMessage = "Product not found.";
                    }
                });
            }
        }

        public AddPantryTransactionPageViewModel(INavigationService navigationService,
            ProductService products):
            base(navigationService, null)
        {
            _navService = navigationService;
            _products = products;
        }
    }
}
