using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PantryTrackers.Views.Recipes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipeListView : ContentView
    {
        public event EventHandler<ItemTappedEventArgs> ItemSelected;

        public RecipeListView()
        {
            InitializeComponent();
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ItemSelected?.Invoke(sender, e);
        }
    }
}