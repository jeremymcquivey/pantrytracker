using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Support.V7.View.Menu;
using Android.Support.V7.Widget;
using App5.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;
using PantryTrackers.Common.Extensions;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigationPageRenderer))]
namespace App5.Droid
{
    public class CustomNavigationPageRenderer : NavigationPageRenderer
    {
        private Toolbar _toolbar;
        private readonly List<ActionMenuView> _menuItemViews =
            new List<ActionMenuView>();

        public CustomNavigationPageRenderer(Context context) : base(context) { }

        public override void OnViewAdded(Android.Views.View child)
        {
            base.OnViewAdded(child);

            if (child.GetType() == typeof(Toolbar))
            {
                _toolbar = (Toolbar)child;
                _toolbar.ChildViewAdded += Toolbar_ChildViewAdded;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _toolbar.ChildViewAdded -= Toolbar_ChildViewAdded;
                foreach(var item in _menuItemViews)
                {
                    item.ChildViewAdded -= TextView_ChildViewAdded;
                }
            }
        }

        private void Toolbar_ChildViewAdded(object sender, ChildViewAddedEventArgs e)
        {
            if (e.Child.GetType() == typeof(ActionMenuView))
            {
                var textView = (ActionMenuView)e.Child;
                _menuItemViews.Add(textView);
                textView.ChildViewAdded += TextView_ChildViewAdded;
            }
            else
            {
                ChangeTextToIcon(e.Child);
            }
        }

        private void TextView_ChildViewAdded(object sender, ChildViewAddedEventArgs e)
        {
            ChangeTextToIcon(e.Child);
        }

        private void ChangeTextToIcon(Android.Views.View view)
        {
            if (view.GetType().IsAssignableFrom(typeof(ActionMenuItemView)))
            {
                var menuItem = (ActionMenuItemView)view;
                var fontType = menuItem.Text.ToFontAwesomeFile();

                if (!string.IsNullOrEmpty(fontType))
                {
                    var myFont = Typeface.CreateFromAsset(Android.App.Application.Context.Assets, $"{fontType}.otf");
                    menuItem.SetTypeface(myFont, TypefaceStyle.Bold);
                }
            }
        }
    }
}