﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PantryTracker.Model.Recipe
{
    public class RecipeIngredient
    {
        public Guid RecipeId { get; set; }

        public int? IngredientId { get; set; }

        public int Index { get; set; }

        public string Quantity { get; set; }

        public string SubQuantity { get; set; }

        public string Unit { get; set; }

        public string Name { get; set; }

        public string Container { get; set; }

        public string Descriptor { get; set; }

        [NotMapped]
        public virtual Ingredient Ingredient { get; set; }

        [NotMapped]
        public virtual Recipe Recipe { get; set; }
    }
}