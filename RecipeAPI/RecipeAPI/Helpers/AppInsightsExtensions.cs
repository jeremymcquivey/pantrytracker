using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System.Collections.Generic;

namespace RecipeAPI.Helpers
{
    /// <summary>
    /// Extends applications insights functionality to clean up event tracking.
    /// </summary>
    public static class AppInsightsExtensions
    {
        /// <summary>
        /// Logs a custom event with no extra properties
        /// </summary>
        public static void LogCustomEvent(this TelemetryClient client, string title)
        {
            LogCustomEvent(client, title, null);
        }

        /// <summary>
        /// Logs a custom event with extra properties
        /// </summary>
        public static void LogCustomEvent(this TelemetryClient client, string title, 
                                          params KeyValuePair<string, string>[] properties)
        {
            var data = new EventTelemetry { Name = title };

            foreach (var prop in properties)
            {
                data.Properties.Add(prop.Key, prop.Value);
            }

            client.TrackEvent(data);
        }
    }
}
