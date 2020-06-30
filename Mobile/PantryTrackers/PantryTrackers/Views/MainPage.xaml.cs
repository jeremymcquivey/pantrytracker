using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Auth;
using Xamarin.Forms;

namespace PantryTrackers.Views
{
    public partial class MainPage : ContentPage
	{
		public MainPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
		}
	}
}