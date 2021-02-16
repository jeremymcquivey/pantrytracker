using PantryTrackers.Models;
using PantryTrackers.ViewModels.Pantry;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PantryTrackers.Views.Pantry
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductSearchPage : ContentPage
    {
        private ProductSearchPageViewModel _model;

        public ProductSearchPage()
        {
            InitializeComponent();

            ItemSearchResults.ItemsSource = new Product[]
            {
                new Product{ Name = "" }
            };
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            
            _model = (ProductSearchPageViewModel)BindingContext;

            if (_model != default)
            {
                _model.OnProductSearchReturned += ProductSearchReturned;
            }
        }

        private void ProductSearchReturned(object sender, IEnumerable<Product> e)
        {
            ItemSearchResults.ItemsSource = new Product[0];
            if (e?.Any() ?? false)
            {
                ItemSearchResults.ItemsSource = e;
            }
        }
    }
}