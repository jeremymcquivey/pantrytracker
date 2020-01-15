namespace PantryTracker.Model.Recipes
{
    /// <summary>
    /// Statuses that define the state of a shared (or private) recipe.
    /// </summary>
    public static class RecipeStates
    {
        /// <summary>
        /// Remains visible only to the owner
        /// </summary>
        public const string Private = "Private";

        /// <summary>
        /// Requested to share with community, but has not yet been approved by a moderator
        /// </summary>
        public const string PendingApproval = "PendingApproval";

        /// <summary>
        /// Publically visible to all registered users
        /// </summary>
        public const string Approved = "Approved";

        /// <summary>
        /// Disapproved by moderator; not publically visible to anyone but the recipe's owner.
        /// </summary>
        public const string Rejected = "Rejected";
    }
}
