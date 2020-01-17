using System;
using PantryTracker.Model.Pantry;
using PantryTracker.Model.Extensions;

namespace PantryTracker.Model.Products
{
    public class RecipeProduct
    {
        public Guid RecipeId { get; set; }

        public string PlainText { get; set; }

        public string Unit { get; set; }

        public string Quantity { get; set; }

        public IngredientMatchType Type { get; set; }

        public Product Product { get; set; }

        public PantryTransaction ToPantryTransaction()
        {
            return new PantryTransaction
            {
                ProductId = Product?.Id ?? 0,
                Quantity = Quantity?.ToNumber() ?? 0,
                Unit = Unit ?? string.Empty
            };
        }
    }
}
