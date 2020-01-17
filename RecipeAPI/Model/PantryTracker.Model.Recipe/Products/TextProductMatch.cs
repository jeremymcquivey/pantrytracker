using System.Collections.Generic;
using PantryTracker.Model.Recipes;

namespace PantryTracker.Model.Products
{
    public class TextProductMatch
    {
        public IEnumerable<RecipeProduct> Matched { get; set; }

        public IEnumerable<Ingredient> Unmatched { get; set; }

        public IEnumerable<Ingredient> Ignored { get; set; }
    }
}
