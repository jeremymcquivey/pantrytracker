using PantryTrackers.Models;
using PantryTrackers.Services;
using PantryTrackers.Views.Pantry;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels.Pantry
{
    internal class ProductSearchPageViewModel : ViewModelBase
    {
        public event EventHandler<IEnumerable<Product>> OnProductSearchReturned;

        private readonly INavigationService _navService;
        private readonly ProductService _products;

        private Command _newProductCommand;
        private Command _productSearchCommand;
        private Command<Product> _productSelectedCommand;
        private Product _selectedProduct;
        private bool _hasSearchResults = true;
        private string _searchText;

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                ProductSelectedCommand.Execute(value);
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                RaisePropertyChanged(nameof(SearchText));
                RaisePropertyChanged(nameof(HasSearchText));
            }
        }

        public bool HasSearchText => !string.IsNullOrEmpty(SearchText);

        public bool HasSearchResults
        {
            get => _hasSearchResults;
            private set
            {
                _hasSearchResults = value;
                RaisePropertyChanged(nameof(HasSearchResults));
            }
        }

        public Command ProductSearchCommand => _productSearchCommand ??=
            new Command(async () =>
            {
                if(string.IsNullOrEmpty(SearchText))
                {
                    return;
                }

                IsNetworkBusy = true;
                var results = await _products.SearchByText(SearchText);
                OnProductSearchReturned?.Invoke(this, results);
                HasSearchResults = results.Any();
                IsNetworkBusy = false;
            }, CanExecute);

        public Command<Product> ProductSelectedCommand => _productSelectedCommand ??=
            new Command<Product>(async product =>
            {
                if(string.IsNullOrEmpty(product?.Name))
                {
                    return;
                }

                var navParams = new NavigationParameters
                {
                    { "SelectedProduct", product }
                };
                await _navService.GoBackAsync(navParams);
            }, (p) => CanExecute());

        public Command NewProductCommand => _newProductCommand ??=
            new Command(async () =>
            {
                IsNetworkBusy = true;
                var navParams = new NavigationParameters
                {
                    { "ProductName", SearchText }
                };

                await _navService.NavigateAsync(nameof(AddProductPage), navParams);
                IsNetworkBusy = false;
            }, CanExecute);

        public ProductSearchPageViewModel(INavigationService navigationService, ProductService products) :
            base(navigationService, null)
        {
            _navService = navigationService;
            _products = products;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("NewProduct"))
            {
                var addedProduct = (Product)parameters["NewProduct"];
                OnProductSearchReturned?.Invoke(this, new List<Product> { addedProduct });
            }
        }

        public override void OnCommandCanExecuteChanged()
        {
            base.OnCommandCanExecuteChanged();

            ProductSelectedCommand.ChangeCanExecute();
            NewProductCommand.ChangeCanExecute();
            ProductSearchCommand.ChangeCanExecute();
        }
    }
}
