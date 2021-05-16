using System.Threading.Tasks;
using PantryTrackers.Controls;
using PantryTrackers.Models;
using PantryTrackers.Models.Meta.Enums;
using PantryTrackers.Models.Products;
using PantryTrackers.Services;
using PantryTrackers.Views.Admin;
using PantryTrackers.Views.Pantry;
using Prism.Navigation;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.Pantry
{
    internal class AddPantryTransactionPageViewModel : ViewModelBase
    {
        private PantryTransaction _transaction;
        private Command _launchBarcodeScannerCommand;
        private Command _saveTransactionCommand;
        private Command _addNewBarcodeCommand;
        private Command _editProductCommand;
        private string _warningMessage;
        private readonly INavigationService _navService;
        private readonly ProductService _products;
        private readonly PantryService _pantry;
        private int _transactionType = TransactionTypes.Usage;

        public Command EditProductCommand => _editProductCommand ??=
            new Command(async () =>
            {
                IsNetworkBusy = true;
                await _navService.NavigateAsync(nameof(ProductSearchPage));
                IsNetworkBusy = false;
            }, CanExecute);

        public Command AddNewBarcodeCommand => _addNewBarcodeCommand ??=
            new Command(async () =>
            {
                IsNetworkBusy = true;
                await _navService.NavigateAsync(nameof(AddBarcodePage), new NavigationParameters
                {
                    { "ProductCode", Transaction.ProductCode },
                });
                IsNetworkBusy = false;
            }, CanExecute);

        public Command LaunchBarcodeScannerCommand => _launchBarcodeScannerCommand ??=
            new Command(async () =>
            {
                IsNetworkBusy = true;
                await _navService.NavigateAsync(nameof(BarcodeScannerPage));
                IsNetworkBusy = false;
            }, CanExecute);

        public Command SaveTransactionCommand => _saveTransactionCommand ??=
            new Command(async () =>
            {
                IsNetworkBusy = true;
                WarningMessage = string.Empty;

                // TODO: Validate at least quantity and product id.

                await Task.Run(async () =>
                {
                    Transaction.TransactionType = _transactionType;
                    var transaction = await _pantry.Save(Transaction);
                    if(transaction != default)
                    {
                        await Device.InvokeOnMainThreadAsync(async () =>
                        {
                            IsNetworkBusy = false;
                            await _navService.GoBackAsync(new NavigationParameters
                            {
                                { "NewTransaction", Transaction }
                            });
                        });
                    }
                    else
                    {
                        await Device.InvokeOnMainThreadAsync(() =>
                        {
                            // TODO Probably validation or technical error.
                            IsNetworkBusy = false;
                            WarningMessage = "Save didn't happen.";
                        });
                    }
                }); 
            }, CanExecute);

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
            get => _transaction ?? (_transaction = new PantryTransaction()); 
            set 
            { 
                _transaction = value; 
                RaisePropertyChanged(nameof(Transaction)); 
            } 
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if(parameters.ContainsKey("SelectedProduct"))
            {
                var newProduct = parameters["SelectedProduct"] as Product;
                if(newProduct != default)
                {
                    Transaction.ProductId = newProduct.Id;
                    Transaction.ProductName = newProduct.Name;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        RaisePropertyChanged(nameof(Transaction));
                    });
                }
            }

            if(parameters.ContainsKey("AddedCode"))
            {
                var newCode = parameters["AddedCode"] as ProductCode;
                if(newCode != default)
                {
                    WarningMessage = string.Empty;
                    Transaction = new PantryTransaction
                    {
                        ProductCode = newCode.Code,
                        Size = newCode.Size,
                        Unit = newCode.Unit,
                        ProductId = newCode.ProductId,
                        ProductName = newCode?.Product?.Name
                    };
                }
            }

            if(parameters.ContainsKey("BarcodeScanResult"))
            {
                WarningMessage = string.Empty;
                await Task.Run(async () =>
                {
                    var productCode = await _products.Search((string)parameters["BarcodeScanResult"]);
                    var product = productCode?.ProductId != default ? await _products.ById(productCode.ProductId) : default;

                    if (productCode != default)
                    {
                        Transaction = new PantryTransaction
                        {
                            ProductCode = (string)parameters["BarcodeScanResult"],
                            Size = productCode.Size,
                            Unit = productCode.Unit,
                            ProductId = productCode.ProductId,
                            ProductName = product?.Name
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

            if(parameters.ContainsKey("TransactionType"))
            {
                _transactionType = (int)parameters["TransactionType"];
            }
        }

        public AddPantryTransactionPageViewModel(INavigationService navigationService,
            ProductService products,
            PantryService pantry):
            base(navigationService, null)
        {
            _navService = navigationService;
            _products = products;
            _pantry = pantry;
        }

        public override void OnCommandCanExecuteChanged()
        {
            base.OnCommandCanExecuteChanged();
            SaveTransactionCommand.ChangeCanExecute();
            LaunchBarcodeScannerCommand.ChangeCanExecute();
            AddNewBarcodeCommand.ChangeCanExecute();
            EditProductCommand.ChangeCanExecute();
        }
    }
}
