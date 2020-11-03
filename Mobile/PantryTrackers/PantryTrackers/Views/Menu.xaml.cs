using PantryTrackers.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PantryTrackers.Views
{
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
                    };

                    btn.Clicked += Button_Clicked;
                    ContentLayout.Children.Add(btn);
                }
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            _vm.DoSomething();
        }
    }
}