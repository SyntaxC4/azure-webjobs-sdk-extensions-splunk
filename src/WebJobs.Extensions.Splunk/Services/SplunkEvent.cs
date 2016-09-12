// Licensed under the MIT License. See License.txt in the project root for license information.

namespace WebJobs.Extensions.Splunk.Services
{
    using System;
    using Newtonsoft.Json;

    public class SplunkEvent
    {
        internal const string AzureWebJobsSplunkHostNameName = "AzureWebJobsSplunkHostName";

        public SplunkEvent()
        {
            Timestamp = DateTime.UtcNow;
        }

        [JsonIgnore]
        public DateTime Timestamp { get; set; }

        public double Time
        {
            get
            {
                double epochTime = (Timestamp - new DateTime(1970, 1, 1)).TotalSeconds;
                return epochTime; // truncate to 3 digits after floating point
            }
        }

        //can be a string, or an object which is serializable by JSON.NET.
        public object Event { get; set; }

        public string Host { get; set; }

        public string Source { get; set; }

        public string SourceType { get; set; }

        public string Index { get; set; }
    }
}