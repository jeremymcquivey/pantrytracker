using System;
using Prism.Navigation;

namespace PantryTrackers.Models.NavMenu
{
    public class NavMenuItem
    {
        public string RequiredRole { get; set; }

        public string Name { get; set; }

        public string NavigationPage { get; set; }
        
        public NavigationParameters Parameters { get; set; }

        public Func<object> CustomMethod { get; set; }
    }
}
