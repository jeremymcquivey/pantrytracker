using System;

namespace RecipeAPI.Model
{
    /// <summary>
    /// Defines the POCO for a user review
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Summary of the review
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Numeric rating given
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Body of the review
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Who submitted the review
        /// </summary>
        public string Reviewer { get; set; }

        // TODO: Create a status i.e. submitted, approved, rejected, removed.

        /// <summary>
        /// Date and time the review was created
        /// </summary>
        public DateTime Date { get; set; }
    }
}
