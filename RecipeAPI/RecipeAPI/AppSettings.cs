namespace RecipeAPI
{
    /// <summary>
    /// Defines all the settings that either need to be stored outside of code but within the application or
    /// all settings that can change between environments.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Connection strings to 3rd party services
        /// </summary>
        public ConnectionString ConnectionStrings { get; set; }

        /// <summary>
        /// Connection strings to 3rd party services
        /// </summary>
        public class ConnectionString
        {
        }
    }
}
