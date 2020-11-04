using PantryTrackers.Extensions;
using PantryTrackers.Models;
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
        private readonly RestClient _client;
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
            new Recipe { Description = "Recipe 1", Tags = new string[]{ "Stuffy", "Cheesy" } },
            new Recipe { Description = "Recipe 2", Tags = new string[]{ "Cheap", "Fast" } }
        });

        public RecipeListPageViewModel(INavigationService navigationService, RestClient client) 
            : base (navigationService)
        {
            Title = "Main Page";
            _nav = navigationService;
            _client = client;
             
            Task.Run(async () =>
            {
                var result = await _client.MakeRequest<object>(new Uri("v1/MenuPlan?startDate=2020-11-03&endDate=2020-11-17", UriKind.Relative), HttpMethod.Get);
                var obj = await result.GetDeserializedContent<object>();
            });
        }
    }
}