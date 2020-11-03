namespace PantryTrackers.Models
{
    internal class MenuItem
    {
        public string RequiredRole { get; set; }

        public string Name { get; set; }

        public string NavigationPage { get; set; }

        public object[] Parameters { get; set; }
    }
}
