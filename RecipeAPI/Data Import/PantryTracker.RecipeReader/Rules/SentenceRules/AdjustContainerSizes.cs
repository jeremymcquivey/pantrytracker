using PantryTracker.Model.Recipes;

namespace PantryTracker.RecipeReader.Rules
{
    public static class IngredientRules
    {
        public static Ingredient AdjustContainerSizes(this Ingredient ingredient)
        {
            if (string.IsNullOrEmpty(ingredient.Quantity) &&
               !string.IsNullOrEmpty(ingredient.Size))
            {
                ingredient.Quantity = ingredient.Size;
                ingredient.Size = string.Empty;
            }

            return ingredient;
        }
    }
}