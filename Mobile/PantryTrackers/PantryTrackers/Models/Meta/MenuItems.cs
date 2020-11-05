using PantryTrackers.Models.NavMenu;
using PantryTrackers.Views.MenuPlan;
using PantryTrackers.Views.Pantry;
using PantryTrackers.Views.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PantryTrackers.Models.Meta
{
    internal static class MenuItems
    {
        public static IEnumerable<NavMenuItem> ForRoles(IEnumerable<string> roles)
        {
            roles ??= Enumerable.Empty<string>();

            return All().Where(menuItem => string.IsNullOrEmpty(menuItem.RequiredRole) ||
                                           roles.Contains(menuItem.RequiredRole));
        }

        private static IEnumerable<NavMenuItem> All()
        {
            return new List<NavMenuItem>
            {
                new NavMenuItem { Name = "Recipes", NavigationPage = nameof(RecipeListPage) },
                new NavMenuItem { Name = "Pantry", NavigationPage = nameof(PantryMainPage) },
                new NavMenuItem { Name = "Menu Plan", NavigationPage = nameof(MenuPlanPage) },
                new NavMenuItem { Name = "Grocery List", NavigationPage = null },
                new NavMenuItem { Name = "--------------", NavigationPage = null },
                new NavMenuItem { Name = "Admin", NavigationPage = null, RequiredRole = "Admin" },
                new NavMenuItem { Name = "Sign Out", NavigationPage = null },
            };
        }
    }
}
