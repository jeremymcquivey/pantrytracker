using PantryTrackers.Common.Enumerations;

namespace PantryTrackers.Common.Extensions
{
    public static class FontAwesomeExtensions
    {
        public static string ToFontAwesomeFile(this string text)
        {
            switch(text?.ToLower() ?? string.Empty)
            {
                case FontAwesomeSolid.Plus:
                case FontAwesomeSolid.Minus:
                case FontAwesomeSolid.Barcode:
                    return nameof(FontAwesomeSolid);
                case "":
                default:
                    return string.Empty;
            }
        }
    }
}
