using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PantryTracker.Model.Recipe
{
    public class Direction
    {
        public Guid RecipeId { get; set; }

        public int Index { get; set; }

        public string Text { get; set; }

        [NotMapped]
        public virtual Recipe Recipe { get; set; }
    }
}
