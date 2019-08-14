using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample.ParseTree.Model
{
    public class UnitsOfMeasure
    {
        private Dictionary<string, List<string>> _unitsOfMeasure =
            new Dictionary<string, List<string>>
            {
                {"Tablespoon", new List<string>() { "tablespoon", "tablespoons", "tbsp" } },
                {"Teaspoon", new List<string>() { "teaspoon", "teaspoons", "tsp" } },
                {"Pinch", new List<string>() { "pinch" } },
                {"Cup", new List<string>() {"cup", "cups"} }
            };

        public bool IsUnitOfMeasure(string word)
        {
            var unit = _unitsOfMeasure.Where(m => m.Value.Contains(word.ToLower()));
            return unit.Any();
        }
    }
}
