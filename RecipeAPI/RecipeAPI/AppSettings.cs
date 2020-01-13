namespace RecipeAPI
{
    /// <summary>
    /// Defines all the settings that either need to be stored outside of code but within the application or
    /// all settings that can change between environments.
    /// </summary>
    public class AppSettings
    {
        public string STSAuthority { get; set; }
    }
}
