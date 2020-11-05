namespace PantryTrackers.Models.NavMenu
{
    public class NavMenuItem
    {
        public string RequiredRole { get; set; }

        public string Name { get; set; }

        public string NavigationPage { get; set; }

        public object[] Parameters { get; set; }
    }
}
