using PantryTrackers.ViewModels;
using Xamarin.Forms;

namespace PantryTrackers.Views.Recipes
{
    public partial class RecipeListPage : ContentPage
	{
		public RecipeListPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
		}

        private void RecipeListView_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            (BindingContext as RecipeListPageViewModel).LoadRecipeDetailsCommand.Execute(e.Item);
        }
    }
}