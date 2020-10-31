
using PantryTrackers.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PantryTrackers.Views.Recipes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipeListCard : ContentView
    {
        public Recipe Recipe { get; private set; }

        public RecipeListCard()
        {
            InitializeComponent();
            BindingContextChanged += RecipeListCard_BindingContextChanged;
        }

        private void RecipeListCard_BindingContextChanged(object sender, System.EventArgs e)
        {
            Recipe = BindingContext as Recipe;
            Recipe.RaisePropertyChanges();
            OnPropertyChanged(nameof(Recipe));
        }
    }
}