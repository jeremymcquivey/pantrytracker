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
        public ServiceEndpoints Endpoints { get; set; }

        /// <summary>
        /// SendGrid template Ids
        /// </summary>
        public EmailConfiguration EmailConfig { get; set; }

        /// <summary>
        /// Connection strings to 3rd party services
        /// </summary>
        public class ServiceEndpoints
        {
        }

        /// <summary>
        /// Defined list of templates needed for application
        /// </summary>
        public class EmailConfiguration
        {
            public string EmailFromName { get; set; }
            public string SampleTemplateId { get; set; }
        }
    }
}
