using PantryTrackers.Common.Extensions;
using PantryTrackers.Models;
using PantryTrackers.Models.Recipes;
using PantryTrackers.Services;
using PantryTrackers.Views.Recipes;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PantryTrackers.ViewModels
{
    public class RecipeListPageViewModel : ViewModelBase
    {
        private readonly INavigationService _nav;
        private BindableCollection<Recipe> _recipes;
        private Command _loadRecipeDetailsCommand;

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
            new Recipe { Title = "Recipe 1", Description = "Very short description of Recipe 1", Tags = new string[]{ "Stuffy", "Cheesy" } },
            new Recipe { Title = "Recipe 2", Description = "Slightly longer (but in reality shorter) description of Recipe 2", Tags = new string[]{ "Cheap", "Fast" } }
        });

        public RecipeListPageViewModel(INavigationService navigationService, RestClient client) 
            : base (navigationService, client)
        {
            //Title = "Main Page";
            _nav = navigationService;
             
            Task.Run(async () =>
            {
                var result = await Client.MakeRequest<object>(new Uri("v1/MenuPlan?startDate=2020-11-03&endDate=2020-11-17", UriKind.Relative), HttpMethod.Get);
                var obj = await result.GetDeserializedContent<object>();
            });
        }
    }
}