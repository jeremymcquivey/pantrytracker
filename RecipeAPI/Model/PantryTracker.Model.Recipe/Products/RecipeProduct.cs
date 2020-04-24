using System;
using PantryTracker.Model.Pantry;
using PantryTracker.Model.Extensions;

namespace PantryTracker.Model.Products
{
    public class RecipeProduct : PantryTransaction
    {
        public Guid RecipeId { get; set; }

        public string PlainText { get; set; }

        public IngredientMatchType MatchType { get; set; }

        public string QuantityString { get; set; }

        public new double Quantity => QuantityString?.ToNumber() ?? 0D;
    }
}
