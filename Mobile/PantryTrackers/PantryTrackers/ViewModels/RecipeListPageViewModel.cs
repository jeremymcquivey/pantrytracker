using PantryTrackers.Models;
using PantryTrackers.Views.Recipes;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Auth;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels
{
    public class RecipeListPageViewModel : ViewModelBase
    {
        private readonly INavigationService _nav;
        private readonly IEnumerable<Account> accounts = null;

        private BindableCollection<Recipe> _recipes;
        private Command _loadRecipeDetailsCommand;

        public string Name => accounts?.FirstOrDefault()?.Properties["firstname"] ?? "Annonymous";

        public Command LoadRecipeDetailsCommand => _loadRecipeDetailsCommand ??= new Command<Recipe>(async (recipe) =>
        {
            var paramList = new NavigationParameters
            {
                { "Recipe", recipe }
            };

            await _nav.NavigateAsync(nameof(RecipeDetailPage), paramList);
        });

        public BindableCollection<Recipe> Recipes => _recipes ??= new BindableCollection<Recipe>(new List<Recipe>
        { 
            new Recipe { Description = "Recipe 1", Tags = new string[]{ "Stuffy", "Cheesy" } },
            new Recipe { Description = "Recipe 2", Tags = new string[]{ "Cheap", "Fast" } }
        });

        public RecipeListPageViewModel(INavigationService navigationService) 
            : base (navigationService)
        {
            Title = "Main Page";

            accounts = AccountStore.Create().FindAccountsForService("AuthServer");
            RaisePropertyChanged(nameof(Name));
            _nav = navigationService;
        }
    }
}