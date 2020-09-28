using System;

namespace PantryTracker.Model.Recipes
{
    public class Ingredient
    {
        public Guid RecipeId { get; set; }

        public int Index { get; set; }

        public string Quantity { get; set; }

        public string Size { get; set; }

        public string Unit { get; set; }

        public string Name { get; set; }

        public string Container { get; set; }

        public string Descriptor { get; set; }

        public override string ToString()
        {
            return $"{Quantity ?? string.Empty} {Size ?? string.Empty} {Unit ?? string.Empty} {Name ?? string.Empty}"
                .Replace("  ", " ")
                .Trim();
        }
    }
}