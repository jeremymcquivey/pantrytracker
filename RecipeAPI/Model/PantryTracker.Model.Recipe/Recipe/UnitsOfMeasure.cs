using System.Collections.Generic;
using System.Linq;

namespace PantryTracker.Model.Recipes
{
    public class UnitsOfMeasure
    {
        private Dictionary<string, List<string>> _unitsOfVolume =
            new Dictionary<string, List<string>>
            {
                {"Tablespoon", new List<string>() { "tablespoon", "tablespoons", "tbsp" } },
                {"Teaspoon", new List<string>() { "teaspoon", "teaspoons", "tsp" } },
                {"Pinch", new List<string>() { "pinch" } },
                {"Cup", new List<string>() { "cup", "cups" } },
                {"Gallon", new List<string>() { "gallon", "gallons", "gal", "gals" } }
            };

        private Dictionary<string, List<string>> _unitsOfWeight =
            new Dictionary<string, List<string>>
            {
                {"Pound", new List<string>() { "lb", "lbs", "pound", "pounds" } },
                {"Ounce", new List<string>() { "oz", "ounce", "ounces" } }
            };

        public bool IsUnitOfMeasure(string word)
        {
            word = new string(word.Where(char.IsLetter).ToArray());

            return _unitsOfVolume.Any(m => m.Value.Contains(word.ToLower())) ||
                   _unitsOfWeight.Any(m => m.Value.Contains(word.ToLower()));
        }
    }
}
