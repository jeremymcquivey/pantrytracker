using System;
namespace PantryTracker.Model.Products
{
    public class UserProductPreference
    {
        public Guid RecipeId { get; set; }

        public string matchingText { get; set; }

        //null denotes an exclusion from the recipe.
        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
