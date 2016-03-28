using System;
using Newtonsoft.Json;

namespace WebJobs.Extensions.Splunk.Services
{
    public class SplunkEvent
    {
        public SplunkEvent()
        {
            Timestamp = DateTime.Now;
        }

        [JsonIgnore]
        public DateTime Timestamp { get; set; }

        public double Time
        {
            get
            {
                double epochTime = (Timestamp.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
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