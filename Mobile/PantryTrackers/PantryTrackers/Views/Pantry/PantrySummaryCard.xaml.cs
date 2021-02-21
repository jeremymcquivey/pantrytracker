using System.Linq;
using PantryTrackers.Models;
using Xamarin.Forms;

namespace PantryTrackers.Views.Pantry
{
    public partial class PantrySummaryCard : ContentView
    {
        private ProductGroup _vm; 

        public PantrySummaryCard()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            _vm = (ProductGroup)BindingContext;

            if(_vm != default)
            {
                ResetList();
            }
        }

        private void ResetList()
        {
            List.Children.Clear();
            foreach(var item in _vm.Elements.OrderBy(item => item.Variety == default))
            {
                List.Children.Add(new Label { Text = item.ToString(_vm.DisplayMode) });
            }
        }
    }
}
