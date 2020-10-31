using PantryTrackers.ViewModels;
using Xamarin.Forms;

namespace PantryTrackers.Views.Recipes
{
    public partial class RecipeListPage : ContentPage
	{
        private RecipeListPageViewModel _vm;

		public RecipeListPage ()
		{
			InitializeComponent ();
            _vm = BindingContext as RecipeListPageViewModel;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
		}

        private void RecipeListView_ItemSelected(object sender, Events.BindableCollectionObjectSelectedArgs<Models.Recipe> e)
        {
            _vm.LoadRecipeDetailsCommand.Execute(e.Data);
        }
    }
}