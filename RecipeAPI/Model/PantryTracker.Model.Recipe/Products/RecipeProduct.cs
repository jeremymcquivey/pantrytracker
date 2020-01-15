using System;
namespace PantryTracker.Model.Products
{
    public class RecipeProduct
    {
        public Product Product { get; set; }

        public Guid RecipeId { get; set; }

        public string PlainText { get; set; }

        public string Unit { get; set; }

        public string Quantity { get; set; }

        public IngredientMatchType Type { get; set; }
    }
}
