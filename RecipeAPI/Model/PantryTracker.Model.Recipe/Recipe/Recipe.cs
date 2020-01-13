using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PantryTracker.Model.Recipe
{
    /// <summary>
    /// Defines the POCO for a recipe object
    /// </summary>
    public class Recipe
    {
        /// <summary>
        /// Unique ID of document
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID of the user who created recipe
        /// </summary>
        public string OwnerId { get; set; }

        /// <summary>
        /// Friendly title of the recipe
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        /// <summary>
        /// Copyright info (if any) for the content of the recipe
        /// </summary>
        public string Credit { get; set; }

        /// <summary>
        /// Main recipe instructions
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Sharing state of recipe. i.e. Public, Private, Rejected, etc...
        /// A recipe must be approved by a moderator to be available shared publically.
        /// </summary>
        public string PublicState { get; set; } = RecipeStates.Private;

        /// <summary>
        /// A list of string tags which categorize the recipe
        /// </summary>
        [NotMapped]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Reviews given for a recipe by app users
        /// </summary>
        [NotMapped]
        public List<Review> Reviews { get; set; }

        /// <summary>
        /// Raw text from imported recipe
        /// </summary>
        public string RawText { get; set; }

        /// <summary>
        /// Prep Time
        /// </summary>
        public string PrepTime { get; set; }

        [MinLength(1)]
        public virtual IEnumerable<Ingredient> Ingredients { get; set; } =
            new List<Ingredient>();

        public virtual IEnumerable<Direction> Directions { get; set; } =
            new List<Direction>();
    }
}