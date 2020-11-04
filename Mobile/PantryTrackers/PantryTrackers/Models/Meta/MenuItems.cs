using PantryTrackers.Views.MenuPlan;
using PantryTrackers.Views.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PantryTrackers.Models.Meta
{
    internal static class MenuItems
    {
        internal static IEnumerable<MenuItem> ForRoles(IEnumerable<string> roles)
        {
            roles ??= Enumerable.Empty<string>();

            return All().Where(menuItem => string.IsNullOrEmpty(menuItem.RequiredRole) ||
                                           roles.Contains(menuItem.RequiredRole));
        }

        private static IEnumerable<MenuItem> All()
        {
            return new List<MenuItem>
            {
                new MenuItem { Name = "Recipes", NavigationPage = nameof(RecipeListPage) },
                new MenuItem { Name = "Pantry", NavigationPage = null },
                new MenuItem { Name = "Menu Plan", NavigationPage = nameof(MenuPlanPage) },
                new MenuItem { Name = "Grocery List", NavigationPage = null },
                new MenuItem { Name = "--------------", NavigationPage = null },
                new MenuItem { Name = "Admin", NavigationPage = null, RequiredRole = "Admin" },
                new MenuItem { Name = "Sign Out", NavigationPage = null },
            };
        }
    }
}
