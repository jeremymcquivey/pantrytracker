using PantryTrackers.Models.Recipes;
using PantryTrackers.Services;
using Prism.Navigation;

namespace PantryTrackers.ViewModels
{
    class RecipeDetailPageViewModel : ViewModelBase, INavigatedAware
    {
        public Recipe Recipe { get; private set; }

        public RecipeDetailPageViewModel(INavigationService navigationService):
            base(navigationService, null)
        {

        }
        public override void OnNavigatedFrom(INavigationParameters parameters) { base.OnNavigatedTo(parameters); }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if(parameters.ContainsKey("Recipe"))
            {
                Recipe = parameters["Recipe"] as Recipe;
                RaisePropertyChanged(nameof(Recipe));
            }
        }
    }
}
