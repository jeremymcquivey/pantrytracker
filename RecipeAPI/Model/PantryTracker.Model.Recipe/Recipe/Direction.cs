using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PantryTracker.Model.Recipes
{
    public class Direction
    {
        public Guid RecipeId { get; set; }

        public int Index { get; set; }

        public string Text { get; set; }
    }
}
