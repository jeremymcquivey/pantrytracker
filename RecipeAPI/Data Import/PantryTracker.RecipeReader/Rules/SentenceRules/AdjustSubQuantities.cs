using PantryTracker.Model.Recipe;

namespace PantryTracker.RecipeReader.Rules
{
    public static class IngredientRules
    {
        public static RecipeIngredient AdjustSubQuantities(this RecipeIngredient ingredient)
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
