using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PantryTracker.Model.Recipe
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [NotMapped]
        public virtual ICollection<Ingredient> Ingredients { get; set; }
    }
}
