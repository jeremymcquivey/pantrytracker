using System;
using PantryTrackers.Models.GroceryList;
using PantryTrackers.ViewModels.GroceryList;
using Xamarin.Forms;

namespace PantryTrackers.Views.GroceryList
{
    public partial class GroceryListMainPage : ContentPage
    {
        private GroceryListMainPageViewModel _vm;

        public GroceryListMainPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            _vm = (GroceryListMainPageViewModel)BindingContext;
        }

        void MenuItem_Clicked(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            // Access the list item through the BindingContext
            var contextItem = menuItem.BindingContext as GroceryListItem;
            var hasQuantity = contextItem.HasQuantity;
            var text = hasQuantity ? $"${menuItem.Text} {Environment.NewLine}Qty: {contextItem.QuantityString}" : menuItem.Text;

            Device.InvokeOnMainThreadAsync(async () =>
            {
                var response = await DisplayActionSheet($"{text}?", null, "Cancel", new[] { "Import to Inventory", "Remove from List" });
                switch (response)
                {
                    case "Mark as Purchased":
                        if (_vm.MarkItemAsPurchasedCommand.CanExecute(contextItem))
                            _vm.MarkItemAsPurchasedCommand.Execute(contextItem);
                        break;
                    case "Remove from List":
                        if (_vm.RemoveItemCommand.CanExecute(contextItem))
                            _vm.RemoveItemCommand.Execute(contextItem);
                        break;
                    case "Import to Inventory":
                        if (_vm.UpdateQuantityCommand.CanExecute(contextItem))
                            _vm.UpdateQuantityCommand.Execute(contextItem);
                        break;
                    case "Cancel":
                    default:
                        break;
                }
            });
        }
    }
}
