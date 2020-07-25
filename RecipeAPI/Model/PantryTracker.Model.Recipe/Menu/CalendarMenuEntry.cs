using PantryTracker.Model.Recipes;
using System;
using System.Collections.Generic;
using System.Text;

namespace PantryTracker.Model.Menu
{
    public class CalendarMenuEntry
    {
        public int Id { get; set; }

        public Guid OwnerId { get; set; }

        public Guid RecipeId { get; set; }

        public DateTime Date { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
