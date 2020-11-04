using PantryTrackers.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PantryTrackers.Views
{
    public class MenuEventArgs: EventArgs
    {
        MenuItem Item { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        private MenuViewModel _vm;

        public Menu()
        {
            InitializeComponent();
            _vm = BindingContext as MenuViewModel;

            foreach(var menuItem in _vm.MenuItems)
            {
                if(string.IsNullOrEmpty(menuItem.NavigationPage))
                {
                    ContentLayout.Children.Add(new Label
                    {
                        Text = menuItem.Name
                    });
                }
                else
                {
                    var btn = new Button
                    {
                        Text = menuItem.Name,
                        Command = _vm.DoSomethingCommand,
                        CommandParameter = menuItem
                    };

                    ContentLayout.Children.Add(btn);
                }
            }
        }
    }
}