using System;
using Sample.ParseTree.Model;

namespace Sample.ParseTree.Rules
{
    public static class IngredientRules
    {
        public static Ingredient AdjustSubQuantities(this Ingredient ingredient)
        {
            if (string.IsNullOrEmpty(ingredient.Quantity) &&
               !string.IsNullOrEmpty(ingredient.SubQuantity))
            {
                ingredient.Quantity = ingredient.SubQuantity;
                ingredient.SubQuantity = string.Empty;
            }

            return ingredient;
        }
    }
}
