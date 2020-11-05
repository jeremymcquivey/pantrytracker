using PantryTrackers.Models.Recipes;
using Prism.Navigation;

namespace PantryTrackers.ViewModels
{
    class RecipeDetailPageViewModel : ViewModelBase, INavigatedAware
    {
        public Recipe Recipe { get; private set; }

        public RecipeDetailPageViewModel(INavigationService navigationService):
            base(navigationService)
        {

        }

        public void OnNavigatedFrom(INavigationParameters parameters) { }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if(parameters.ContainsKey("Recipe"))
            {
                Recipe = parameters["Recipe"] as Recipe;
                RaisePropertyChanged(nameof(Recipe));
            }
        }
    }
}
