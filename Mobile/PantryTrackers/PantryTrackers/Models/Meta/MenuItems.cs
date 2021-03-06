﻿using PantryTrackers.Models.NavMenu;
using PantryTrackers.Views.Admin;
using PantryTrackers.Views.GroceryList;
using PantryTrackers.Views.MenuPlan;
using PantryTrackers.Views.NavMenu;
using PantryTrackers.Views.Pantry;
using PantryTrackers.Views.Recipes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PantryTrackers.Models.Meta
{
    internal static class MenuItems
    {
        private static IEnumerable<NavMenuItem> _allMenuItems = new List<NavMenuItem>
        {
            new NavMenuItem { Name = "Recipes", NavigationPage = nameof(RecipeListPage) },
            new NavMenuItem { Name = "Pantry", NavigationPage = nameof(PantryMainPage) },
            new NavMenuItem { Name = "Menu Plan", NavigationPage = nameof(MenuPlanPage) },
            new NavMenuItem { Name = "Grocery List", NavigationPage = nameof(GroceryListMainPage) },
            new NavMenuItem { Name = "--------------", NavigationPage = null },
            new NavMenuItem { Name = "Admin", NavigationPage = nameof(AdminMenuPage), RequiredRole = "Admin" },
            new NavMenuItem { Name = "Sign Out", NavigationPage = null }
        };

        public static IEnumerable<NavMenuItem> ForRoles(IEnumerable<string> roles)
        {
            roles ??= Enumerable.Empty<string>();

            return _allMenuItems.Where(menuItem => string.IsNullOrEmpty(menuItem.RequiredRole) ||
                                           roles.Contains(menuItem.RequiredRole));
        }

        public static IEnumerable<NavMenuItem> Admin()
        {
            return new List<NavMenuItem>
            {
                new NavMenuItem { Name = "Products", NavigationPage = nameof(ProductSearchPage) },
                new NavMenuItem { Name = "Bar Code Maps", NavigationPage = nameof(AddBarcodePage) },
            };
        }
    }
}
