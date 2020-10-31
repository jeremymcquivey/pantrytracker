using PantryTrackers.Models;
using PantryTrackers.Views.Recipes;
using Prism.Navigation;

namespace PantryTrackers.ViewModels
{
    public class MenuViewModel: ViewModelBase
    {
        private INavigationService _navService;

        public Recipe Recipe { get; private set; }

        public MenuViewModel(INavigationService navigationService)
            : base(navigationService) 
        {
            _navService = navigationService;
        }

        public async void DoSomething() 
        {
            await _navService.NavigateAsync(nameof(RecipeListPage));
        }
    }
}
