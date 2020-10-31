using PantryTrackers.Events;
using PantryTrackers.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PantryTrackers.Views.Recipes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipeListView : ContentView
    {
        public event EventHandler<BindableCollectionObjectSelectedArgs<Recipe>> ItemSelected;

        public BindableCollection<Recipe> Recipes { get; private set; }

        public RecipeListView()
        {
            InitializeComponent();
            BindingContextChanged += RecipeListView_BindingContextChanged;
        }

        private void RecipeListView_BindingContextChanged(object sender, EventArgs e)
        {
            Recipes = BindingContext as BindableCollection<Recipe>;

            ListContainer.Children.Clear();
            foreach (var recipe in Recipes.Collection)
            {
                recipe.CardSelectedCommand = new Command((recipe) => 
                {
                    ItemSelected?.Invoke(this, new BindableCollectionObjectSelectedArgs<Recipe>(recipe as Recipe));
                });
                
                ListContainer.Children.Add(new RecipeListCard()
                {
                    BindingContext = recipe,
                });
            }
        }
    }
}