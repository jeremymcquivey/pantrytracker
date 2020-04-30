using System.Collections.Generic;
using PantryTracker.Model.Recipes;

namespace PantryTracker.Model.Products
{
    public class TextProductMatch
    {
        public IEnumerable<RecipeProduct> Matched { get; set; }

        public IEnumerable<RecipeProduct> Unmatched { get; set; }

        public IEnumerable<RecipeProduct> Ignored { get; set; }
    }
}
