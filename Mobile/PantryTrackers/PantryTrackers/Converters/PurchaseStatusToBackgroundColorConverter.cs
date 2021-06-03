using System;
using System.Globalization;
using PantryTrackers.Models.GroceryList;
using Xamarin.Forms;

namespace PantryTrackers.Converters
{
    public class PurchaseStatusToBackgroundColorConverter : IValueConverter
    {
        private Color ColorToUse = Color.LightGray;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((GroceryListItemStatus)value)
            {
                case GroceryListItemStatus.Purchased:
                    return ColorToUse;
                default:
                    return TextDecorations.None;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return GroceryListItemStatus.Active;
        }
    }
}
