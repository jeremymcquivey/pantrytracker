using PantryTracker.Model.Recipes;
using System;
namespace PantryTracker.Model.Products
{
    public class UserProductPreference
    {
        public int Id { get; set; }

        public Guid RecipeId { get; set; }

        public string matchingText { get; set; }

        //null denotes an exclusion from the recipe.
        public int? ProductId { get; set; }

        public int? VarietyId { get; set; }

        public virtual Product Product { get; set; }

        public virtual ProductVariety Variety { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
