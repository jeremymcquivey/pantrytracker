namespace RecipeAPI.Model
{
    /// <summary>
    /// Defines the base contract for movement between the application and the document store.
    /// </summary>
    public interface IDocument
    {
        /// <summary>
        /// Assigned unique ID (can be set manually or deferred to document store).
        /// </summary>
        string Id { get; set; }
    }
}
